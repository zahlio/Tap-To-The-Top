using UnityEngine;
using System.Collections;
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif



public class IOSNativeUtility {


	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _redirectToAppStoreRatingPage(string appId);

	[DllImport ("__Internal")]
	private static extern void _ISNShowPreloader();


	[DllImport ("__Internal")]
	private static extern void _ISNHidePreloader();
	#endif


	public static void RedirectToAppStoreRatingPage() {
		RedirectToAppStoreRatingPage(IOSNativeSettings.Instance.AppleId);
	}

	public static void RedirectToAppStoreRatingPage(string appleId) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_redirectToAppStoreRatingPage(appleId);
		#endif
	}


	public static void ShowPreloader() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_ISNShowPreloader();
		#endif
	}
	
	public static void HidePreloader() {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_ISNHidePreloader();
		#endif
	}
}
