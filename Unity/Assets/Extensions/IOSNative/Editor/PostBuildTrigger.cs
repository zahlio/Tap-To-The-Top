﻿using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using System.Collections;
using System.IO;
using System;

public static class PostBuildTrigger
	
{
	
	// Frameworks Ids  -  These ids have been generated by creating a project using Xcode then
	// extracting the values from the generated project.pbxproj.  The format of this
	// file is not documented by Apple so the correct algorithm for generating these
	// ids is unknown


	const string CORETELEPHONY_ID = "919BD0C3159C677000C931BE" ;
	const string CORETELEPHONY_FILEREFID = "919BD0C2159C677000C931BE" ;
	
	
	
	// List of all the frameworks to be added to the project
	
	public struct framework
		
	{
		
		public string sName ;
		
		public string sId ;
		
		public string sFileId ;
		
		
		
		public framework(string name, string myId, string fileid)
		
		{
			
			sName = name ;
			
			sId = myId ;
			
			sFileId = fileid ;
			
		}
		
	}
	
	
	
	
	
	/// Processbuild Function
	
//	[PostProcessBuild] // <- this is where the magic happens
	
	public static void OnPostProcessBuild(BuildTarget target, string path)
		
	{

		Debug.Log("builded");
		//return;
		
		// 1: Check this is an iOS build before running
		
		#if UNITY_IPHONE
		

			// 2: We init our tab and process our project
			framework[] myFrameworks = { new framework("CoreTelephony.framework", CORETELEPHONY_ID, CORETELEPHONY_FILEREFID) } ;
			string xcodeprojPath = Application.dataPath ;
			xcodeprojPath = xcodeprojPath.Substring(0, xcodeprojPath.Length - 16) ;
			string device = "iOSandIpad" ;
			if (PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPadOnly)
				device = "iPad" ;
			else if (PlayerSettings.iOS.targetDevice == iOSTargetDevice.iPhoneOnly)
				device = "iPhone" ;
			xcodeprojPath = xcodeprojPath + "buildios/"+device+"/Unity-iPhone.xcodeproj" ;
			
			//          Debug.Log("We found xcodeprojPath to be : "+xcodeprojPath) ;
			
			
			
			Debug.Log("OnPostProcessBuild - START on: "+device) ;
			
			updateXcodeProject(xcodeprojPath, myFrameworks) ;                       
			


		
		#endif      
		
		Debug.Log("OnPostProcessBuild - STOP") ;
		
	}
	
	
	
	
	
	
	
	
	
	// MAIN FUNCTION
	// xcodeproj_filename - filename of the Xcode project to change
	// frameworks - list of Apple standard frameworks to add to the project
	
	public static void updateXcodeProject(string xcodeprojPath, framework[] listeFrameworks) {
		
		// STEP 1 : We open up the file generated by Unity and read into memory as
		// a list of lines for processing
		string project = xcodeprojPath + "/project.pbxproj" ;
		string[] lines = System.IO.File.ReadAllLines(project);
		

		// STEP 2 : We check if file has already been processed and only proceed if it hasn't,
		// we'll do this by looping through the build files and see if CoreTelephony.framework
		// is there
		
		int i = 0 ;
		bool bFound = false ;
		bool bEnd = false ;
		while ( !bFound && !bEnd) {
			if (lines[i].Length > 5 && (String.Compare(lines[i].Substring(3, 3), "End") == 0) )
				bEnd = true ;
			bFound = lines[i].Contains("CoreTelephony.framework") ;
			++i ;
		}
		if (bFound)	
			Debug.Log("OnPostProcessBuild - ERROR: Frameworks have already been added to XCode project") ;
		else {
			
			// STEP 3 : We'll open/replace project.pbxproj for writing and iterate over the old
			
			// file in memory, copying the original file and inserting every extra we need
			
			FileStream filestr = new FileStream(project, FileMode.Create); //Create new file and open it for read and write, if the file exists overwrite it.
			filestr.Close() ;
			StreamWriter fCurrentXcodeProjFile = new StreamWriter(project) ; // will be used for writing
			
			
			
			// As we iterate through the list we'll record which section of the
			// project.pbxproj we are currently in
			string section = "" ;
			
			
			
			// We use this boolean to decide whether we have already added the list of
			// build files to the link line.  This is needed because there could be multiple
			// build targets and they are not named in the project.pbxproj
			
			bool bFrameworks_build_added = false ;
			int iNbBuildConfigSet = 0 ; // can't be > 2
			
			
			i = 0 ;
			foreach (string line in lines) {
			//	if (line.StartsWith("\t\t\t\tGCC_ENABLE_CPP_EXCEPTIONS") ||  line.StartsWith("\t\t\t\tGCC_ENABLE_CPP_RTTI") ||  line.StartsWith("\t\t\t\tGCC_ENABLE_OBJC_EXCEPTIONS") ){
					
					// apparently, we don't copy those lines in our new project
					
				//} else {                           
					
		
					fCurrentXcodeProjFile.WriteLine(line) ;
					

					
					//////////////////////////////////
					
					//  STEP 2 : Include Framewoks  //
					
					//////////////////////////////////

					
					// Each section starts with a comment such as : /* Begin PBXBuildFile section */'
					
					if ( lines[i].Length > 7 && String.Compare(lines[i].Substring(3, 5), "Begin") == 0  )
						
					{
						
						section = line.Split(' ')[2] ;
						
						//Debug.Log("NEW_SECTION: "+section) ;
						
						if (section == "PBXBuildFile")
							
						{
							
							foreach (framework fr in listeFrameworks)
								
								add_build_file(fCurrentXcodeProjFile, fr.sId, fr.sName, fr.sFileId) ;
							
						}
						
						
						
						if (section == "PBXFileReference")
							
						{
							
							foreach (framework fr in listeFrameworks)
								
								add_framework_file_reference(fCurrentXcodeProjFile, fr.sFileId, fr.sName) ;
							
						}
						
						
						
						if (line.Length > 5 && String.Compare(line.Substring(3, 3), "End") == 0)
							
							section = "" ;
						
					}
					
					// The PBXResourcesBuildPhase section is what appears in XCode as 'Link
					
					// Binary With Libraries'.  As with the frameworks we make the assumption the
					
					// first target is always 'Unity-iPhone' as the name of the target itself is
					
					// not listed in project.pbxproj                
					
					if (section == "PBXFrameworksBuildPhase" &&
					    
					    line.Trim().Length > 4 &&
					    
					    String.Compare(line.Trim().Substring(0, 5) , "files") == 0 &&
					    
					    !bFrameworks_build_added)
						
					{
						
						foreach (framework fr in listeFrameworks)
							
							add_frameworks_build_phase(fCurrentXcodeProjFile, fr.sId, fr.sName) ;
						
						bFrameworks_build_added = true ;
						
					}
					
					
					
					// The PBXGroup is the section that appears in XCode as 'Copy Bundle Resources'.            
					
					if (section == "PBXGroup" &&
					    
					    line.Trim().Length > 7 && 
					    
					    String.Compare(line.Trim().Substring(0, 8) , "children") == 0 &&
					    
					    lines[i-2].Trim().Split(' ').Length > 0 &&
					    
					    String.Compare(lines[i-2].Trim().Split(' ')[2] , "CustomTemplate" ) == 0 )
						
					{
						
						foreach (framework fr in listeFrameworks)
							
							add_group(fCurrentXcodeProjFile, fr.sFileId, fr.sName) ;
						
					}
					
					
					
					//////////////////////////////
					
					//  STEP 3 : Build Options  //
					
					//////////////////////////////
					
					//
					
					// AdColony needs "Other Linker Flags" to have "-all_load -ObjC" added to its value
					
					//
					
					//////////////////////////////
					
					if (section == "XCBuildConfiguration" &&
					    
					    line.StartsWith("\t\t\t\tOTHER_LDFLAGS") &&
					    
					    iNbBuildConfigSet < 2)
						
					{
						
						//fCurrentXcodeProjFile.Write("\t\t\t\t\t\"-all_load\",\n") ;
						
						fCurrentXcodeProjFile.Write("\t\t\t\t\t\"-ObjC\",\n") ;
						
						Debug.Log("OnPostProcessBuild - Adding \"-ObjC\" flag to build options") ; // \"-all_load\" and
						
						++iNbBuildConfigSet ;
						
					}
					
			//	}
				
				++i ;
				
			}
			
			fCurrentXcodeProjFile.Close() ;
			
		}
		
	}
	
	
	
	
	
	/////////////////
	
	///////////
	
	// ROUTINES
	
	///////////
	
	/////////////////
	
	
	
	
	
	// Adds a line into the PBXBuildFile section
	
	private static void add_build_file(StreamWriter file, string id, string name, string fileref)
		
	{
		
		Debug.Log("OnPostProcessBuild - Adding build file " + name) ;
		
		string subsection = "Frameworks" ;
		
		
		
		if (name == "CoreTelephony.framework")  // CoreTelephony.framework should be weak-linked
			
			file.Write("\t\t"+id+" /* "+name+" in "+subsection+" */ = {isa = PBXBuildFile; fileRef = "+fileref+" /* "+name+" */; settings = {ATTRIBUTES = (Weak, ); }; };") ;
		
		else // Others framework are normal
			
			file.Write("\t\t"+id+" /* "+name+" in "+subsection+" */ = {isa = PBXBuildFile; fileRef = "+fileref+" /* "+name+" */; };\n") ;
		
	}
	
	
	
	// Adds a line into the PBXBuildFile section
	
	private static void add_framework_file_reference(StreamWriter file, string id, string name)
		
	{
		
		Debug.Log("OnPostProcessBuild - Adding framework file reference " + name) ;
		
		
		
		string path = "System/Library/Frameworks" ; // all the frameworks come from here
		
		if (name == "libsqlite3.0.dylib")           // except for lidsqlite
			
			path = "usr/lib" ;
		
		
		
		file.Write("\t\t"+id+" /* "+name+" */ = {isa = PBXFileReference; lastKnownFileType = wrapper.framework; name = "+name+"; path = "+path+"/"+name+"; sourceTree = SDKROOT; };\n") ;
		
	}
	
	
	
	// Adds a line into the PBXFrameworksBuildPhase section
	
	private static void add_frameworks_build_phase(StreamWriter file, string id, string name)
		
	{
		
		Debug.Log("OnPostProcessBuild - Adding build phase " + name) ;
		
		
		
		file.Write("\t\t\t\t"+id+" /* "+name+" in Frameworks */,\n") ;
		
	}
	
	
	
	// Adds a line into the PBXGroup section
	
	private static void add_group(StreamWriter file, string id, string name)
		
	{
		
		Debug.Log("OnPostProcessBuild - Add group " + name) ;
		
		
		
		file.Write("\t\t\t\t"+id+" /* "+name+" */,\n") ;
		
	}
	
}