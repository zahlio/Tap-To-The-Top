////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class IOSNotificationController : EventDispatcher  {


	private static IOSNotificationController _instance;

	public const string DEVICE_TOKEN_RECEIVED = "device_token_received";

	#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _scheduleNotification (int time, string message, bool sound);
	
	[DllImport ("__Internal")]
	private static extern  void _showNotificationBanner (string title, string messgae);
	
	[DllImport ("__Internal")]
	private static extern void _cancelNotifications();
	#endif

	

	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	public static IOSNotificationController instance {

		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType(typeof(IOSNotificationController)) as IOSNotificationController;
				if (_instance == null) {
					_instance = new GameObject ("IOSNotificationController").AddComponent<IOSNotificationController> ();
				}
			}

			return _instance;

		}

	}

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	public void ShowNotificationBanner (string title, string messgae) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_showNotificationBanner (title, messgae);
		#endif
	}

	public void CancelNotifications () {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_cancelNotifications();
		#endif
	}

	public void ScheduleNotification(int time, string message, bool sound) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE
			_scheduleNotification (time, message, sound);
		#endif
	}


	
	
	#if UNITY_IPHONE
	public void RegisterForRemoteNotifications(RemoteNotificationType notificationTypes) {
		#if (UNITY_IPHONE && !UNITY_EDITOR) || SA_DEBUG_MODE

			NotificationServices.RegisterForRemoteNotificationTypes(notificationTypes);
			DeviceTokenListner.Create ();
		#endif
	}
	
	#endif

	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	public void OnDeviceTockeReceived(IOSNotificationDeviceToken token) {
		dispatch (DEVICE_TOKEN_RECEIVED, token);
	}
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
