using UnityEngine;
using System.Collections;
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class IOSNativePopUpManager {

	//--------------------------------------
	//  NATIVE FUNCTIONS
	//--------------------------------------
	
	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _showRateUsPopUp(string title, string message, string rate, string remind, string declined);
	
	[DllImport ("__Internal")]
	private static extern void _showDialog(string title, string message, string yes, string no);
	
	[DllImport ("__Internal")]
	private static extern void _showMessage(string title, string message, string ok);
	
	[DllImport ("__Internal")]
	private static extern void _dismissCurrentAlert();
	#endif


	public static void dismissCurrentAlert() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_dismissCurrentAlert();
		#endif
		
		
	}
	
	
	public static void showRateUsPopUP(string title, string message, string rate, string remind, string declined) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_showRateUsPopUp(title, message, rate, remind, declined);
		#endif
	}
	
	
	public static void showDialog(string title, string message) {
		showDialog(title, message, "Yes", "No");
	}
	
	public static void showDialog(string title, string message, string yes, string no) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_showDialog(title, message, yes, no);
		#endif
	}
	
	
	public static void showMessage(string title, string message) {
		showMessage(title, message, "Ok");
	}
	
	public static void showMessage(string title, string message, string ok) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_showMessage(title, message, ok);
		#endif
	}
}

