////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////




using UnityEngine;
using System.Collections;
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class IOSSocialManager : EventDispatcher {

	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _twPost(string text);
	
	[DllImport ("__Internal")]
	private static extern void _twPostWithMedia(string text, string encodedMedia);
	

	[DllImport ("__Internal")]
	private static extern void _fbPost(string text);
	
	[DllImport ("__Internal")]
	private static extern void _fbPostWithMedia(string text, string encodedMedia);

	[DllImport ("__Internal")]
	private static extern void _mediaShare(string text, string encodedMedia);

	#endif

	private static IOSSocialManager _instance = null;


	
	public const string TWITTER_POST_FAILED  = "twitter_post_failed";
	public const string TWITTER_POST_SUCCESS = "twitter_post_success";
	
	public const string FACEBOOK_POST_FAILED  = "facebook_post_failed";
	public const string FACEBOOK_POST_SUCCESS = "facebook_post_success";


	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}



	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public void ShareMedia(string text) {
		ShareMedia(text, null);
	}

	public void ShareMedia(string text, Texture2D texture) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			if(texture != null) {
				byte[] val = texture.EncodeToPNG();
				string bytesString = System.Convert.ToBase64String (val);
				_mediaShare(text, bytesString);
			} else {
				_mediaShare(text, "");
			}
		#endif
	}

	public void TwitterPost(string text) {
		TwitterPost(text, null);
	}


	public void TwitterPost(string text, Texture2D texture) {
		if(texture == null) {
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
				_twPost(text);
			#endif
		} else {


			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
				byte[] val = texture.EncodeToPNG();
				string bytesString = System.Convert.ToBase64String (val);

				_twPostWithMedia(text, bytesString);
			#endif
		}

	}


	public void FacebookPost(string text) {
		FacebookPost(text, null);
	}
	
	public void FacebookPost(string text, Texture2D texture) {
		if(texture == null) {
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
				_fbPost(text);
			#endif
		} else {

			
			#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
				byte[] val = texture.EncodeToPNG();
				string bytesString = System.Convert.ToBase64String (val);
				_fbPostWithMedia(text, bytesString);
			#endif
		}
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	public static IOSSocialManager instance  {
		get {
			if(_instance == null) {
				GameObject go =  new GameObject("IOSSocialManager");
				_instance = go.AddComponent<IOSSocialManager>();
			}

			return _instance;
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void OnTwitterPostFailed() {
		dispatch(TWITTER_POST_FAILED);
	}

	private void OnTwitterPostSuccess() {
		dispatch(TWITTER_POST_SUCCESS);
	}

	private void OnFacebookPostFailed() {
		dispatch(FACEBOOK_POST_FAILED);
	}
	
	private void OnFacebookPostSuccess() {
		dispatch(FACEBOOK_POST_SUCCESS);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------


	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
