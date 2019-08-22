﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using Firebase.Messaging;

namespace Plugin.FirebasePushNotification
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class PNFirebaseIIDService : FirebaseMessagingService
	{
		/**
         * Called when token is updated. This may occur if the security of
         * the previous token had been compromised. Note that this is called when the Instance token
         * is initially generated so this is where you would retrieve the token.
         */
		public override void OnNewToken(string p0)
		{
			base.OnNewToken(p0);
		
            var refreshedToken = p0;

            //If previous token is null or empty resubscribe to topics since the old instance id isn't valid anymore
            if(string.IsNullOrEmpty(CrossFirebasePushNotification.Current.Token))
            {
                foreach (var t in CrossFirebasePushNotification.Current.SubscribedTopics)
                {
                    FirebaseMessaging.Instance.SubscribeToTopic(t);
                }

            }
            
            var editor = Android.App.Application.Context.GetSharedPreferences(FirebasePushNotificationManager.KeyGroupName, FileCreationMode.Private).Edit();
            editor.PutString(FirebasePushNotificationManager.FirebaseTokenKey, refreshedToken);
            editor.Commit();
     
           // CrossFirebasePushNotification.Current.OnTokenRefresh?.Invoke(this,refreshedToken);
            FirebasePushNotificationManager.RegisterToken(refreshedToken);
            System.Diagnostics.Debug.WriteLine($"REFRESHED TOKEN: {refreshedToken}");
        }
    }
}