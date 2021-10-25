﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using EndProjectApp.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;

namespace EndProjectApp.Services
{
    class EndProjectAPIProxy
    {
        private const string CLOUD_URL = "TBD"; //API url when going on the cloud
        private const string CLOUD_PHOTOS_URL = "TBD";
        private const string DEV_ANDROID_EMULATOR_URL = "http://10.0.2.2:45547/EndProjectAPI"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://192.168.1.14:45547/EndProjectAPI"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "https://localhost:44368/EndProjectAPI"; //API url when using windoes on development
        private const string DEV_ANDROID_EMULATOR_PHOTOS_URL = "http://10.0.2.2:45547/Images/"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://192.168.1.14:45547/Images/"; //API url when using physucal device on android
        private const string DEV_WINDOWS_PHOTOS_URL = "https://localhost:44368/Images/"; //API url when using windoes on development

        private HttpClient client;
        private string baseUri;
        private string basePhotosUri;
        private static EndProjectAPIProxy proxy = null;

        public static EndProjectAPIProxy CreateProxy()
        {
            string baseUri;
            string basePhotosUri;
            if (App.IsDevEnv)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (DeviceInfo.DeviceType == DeviceType.Virtual)
                    {
                        baseUri = DEV_ANDROID_EMULATOR_URL;
                        basePhotosUri = DEV_ANDROID_EMULATOR_PHOTOS_URL;
                    }
                    else
                    {
                        baseUri = DEV_ANDROID_PHYSICAL_URL;
                        basePhotosUri = DEV_ANDROID_PHYSICAL_PHOTOS_URL;
                    }
                }
                else
                {
                    baseUri = DEV_WINDOWS_URL;
                    basePhotosUri = DEV_WINDOWS_PHOTOS_URL;
                }
            }
            else
            {
                baseUri = CLOUD_URL;
                basePhotosUri = CLOUD_PHOTOS_URL;
            }

            if (proxy == null)
                proxy = new EndProjectAPIProxy(baseUri, basePhotosUri);
            return proxy;
        }


        private EndProjectAPIProxy(string baseUri, string basePhotosUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
            this.basePhotosUri = basePhotosUri;
        }

        public string GetBasePhotoUri() { return this.basePhotosUri; }

        //Login!
        public async Task<User> LoginAsync(string email, string pass)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/Login?Email={email}&Password={pass}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    User u = JsonSerializer.Deserialize<User>(content, options);
                    return u;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<bool> SignUpAsync(User u)
        {
            string userJson = JsonSerializer.Serialize(u);
            StringContent stringContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUp", stringContent);
            return response.IsSuccessStatusCode;
        }



        //Upload file to server (only images!)
        //public async Task<bool> UploadImage(Models.FileInfo fileInfo, string targetFileName)
        //{
        //    try
        //    {
        //        var multipartFormDataContent = new MultipartFormDataContent();
        //        var fileContent = new ByteArrayContent(File.ReadAllBytes(fileInfo.Name));
        //        multipartFormDataContent.Add(fileContent, "file", targetFileName);
        //        HttpResponseMessage response = await client.PostAsync($"{this.baseUri}/UploadImage", multipartFormDataContent);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return true;
        //        }
        //        else
        //            return false;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return false;
        //    }
        //}

    }
}