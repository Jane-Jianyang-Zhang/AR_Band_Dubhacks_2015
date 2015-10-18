/*
    Copyright (c) Xpert360 Ltd All rights reserved.  
 
    MIT License: 
 
    Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
    documentation files (the  "Software"), to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
    and to permit persons to whom the Software is furnished to do so, subject to the following conditions: 
 
    The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. 
 
    THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
    TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Band;
using Microsoft.Band.Sensors;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using System.Collections.Generic;
using Windows.UI.Popups;
using System.Linq;
using Windows.Web.Http;
using System.IO;
using System.Runtime.Serialization.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BandSensors
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            stackHeartRate = new Stack<int>();
            stackCalories = new Stack<long>();
            stackSkinTemperature = new Stack<double>();
            stackUV = new Stack<string>();
            stackPedometer = new Stack<long>();
            stackDistance = new Stack<long>();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

        }
        IBandClient bandClient;
        DispatcherTimer timer;
        Stack<int> stackHeartRate;
        Stack<long> stackCalories;
        Stack<double> stackSkinTemperature;
        Stack<string> stackUV;
        Stack<long> stackPedometer;
        Stack<long> stackDistance;
        string code;


        private async void Timer_Tick(object sender, object e)
        {
            Data data = new Data();
            if (stackCalories.Count > 0)
            {
                data.Calorie = stackCalories.Pop();

            }
            if (stackDistance.Count > 0)
            {
                data.Distance = stackDistance.Pop();
            };
            if (stackHeartRate.Count > 0)
            {
                data.HeartRate = stackHeartRate.Pop();
            }
            if (stackSkinTemperature.Count > 0)
            {
                data.SkinTemperature = stackSkinTemperature.Pop();
            }
            if (stackPedometer.Count > 0)
            {
                data.Steps = stackPedometer.Pop();
            }
            if (stackUV.Count > 0)
            {

                data.UV = stackUV.Pop();
            }


            HttpClient httpClient = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, new Uri("https://glowing-inferno-4375.firebaseio.com/" + code + ".json"));

            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Data));
            ser.WriteObject(stream1, data);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);

            message.Content = new HttpStringContent(sr.ReadToEnd());
            HttpResponseMessage response = await httpClient.SendRequestAsync(message);
        }


        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async void ButtonRun_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                // Get the list of Microsoft Bands paired to the phone.
                IBandInfo[] pairedBands = await BandClientManager.Instance.GetBandsAsync();
                if (pairedBands.Length < 1)
                {
                    MessageDialog dialog = new MessageDialog("This sample app requires a Microsoft Band paired to your device.");
                    await dialog.ShowAsync();
                    return;
                }

                // Connect to Microsoft Band.
                bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]);


                if (bandClient.SensorManager.HeartRate.GetCurrentUserConsent() != UserConsent.Granted)
                {
                    await bandClient.SensorManager.HeartRate.RequestUserConsentAsync();
                }

                int samplesReceivedAcc = 0; // the number of Accelerometer samples received
                int samplesReceivedCal = 0; // the number of Calories samples received
                int samplesReceivedCon = 0; // the number of Contact samples received
                int samplesReceivedDist = 0; // the number of Contact samples received
                int samplesReceivedGyro = 0; // the number of Gyroscope samples received
                int samplesReceivedHR = 0; // the number of HeartRate samples received
                int samplesReceivedPed = 0; // the number of Pedometer samples received
                int samplesReceivedST = 0; // the number of SkinTemperature samples received
                int samplesReceivedUV = 0; // the number of UV samples received


                //code = RandomString(5);
                code = "123";

                //MessageDialog codeDialog = new MessageDialog("Your code is: " + code + ".");
                //await codeDialog.ShowAsync();



                timer.Start();



                // Subscribe to Accelerometer data.
                bandClient.SensorManager.Accelerometer.ReadingChanged += async (s, args) =>
                {
                    samplesReceivedAcc++;

                    // Only report occasional Accelerometer data
                    IBandAccelerometerReading readings = args.SensorReading;
                    
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        this.txtAccelerometer.Text = readings.AccelerationX.ToString() + " " + readings.AccelerationY.ToString() + " " + readings.AccelerationZ.ToString();
                    });

                };
                await bandClient.SensorManager.Accelerometer.StartReadingsAsync();

                // Subscribe to Calories data.
                bandClient.SensorManager.Calories.ReadingChanged += async (s, args) =>
                {
                    samplesReceivedCal++;
                    IBandCaloriesReading readings = args.SensorReading;
                    stackCalories.Push(readings.Calories);
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        this.txtCalories.Text = readings.Calories.ToString();
                    });
                };
                await bandClient.SensorManager.Calories.StartReadingsAsync();

                // Subscribe to Contact data.
                bandClient.SensorManager.Contact.ReadingChanged += async (s, args) =>
                {
                    samplesReceivedCon++;
                    IBandContactReading readings = args.SensorReading;
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        this.txtContact.Text = readings.State.ToString();
                    });
                };
                await bandClient.SensorManager.Contact.StartReadingsAsync();

                // Subscribe to Distance data.
                bandClient.SensorManager.Distance.ReadingChanged += async (s, args) =>
                {
                    samplesReceivedDist++;
                    IBandDistanceReading readings = args.SensorReading;
                    stackDistance.Push(readings.TotalDistance);
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        this.txtDistance.Text = readings.CurrentMotion.ToString() + " " + readings.Pace.ToString() + " " + readings.Speed.ToString() + " " + readings.TotalDistance.ToString();
                    });
                };
                await bandClient.SensorManager.Distance.StartReadingsAsync();

                // Subscribe to Gyroscope data.
                bandClient.SensorManager.Gyroscope.ReadingChanged += async (s, args) =>
                {
                    samplesReceivedGyro++;
                    if ((samplesReceivedGyro % 20) == 0)
                    {
                        // Only report occasional Gyroscope data
                        IBandGyroscopeReading readings = args.SensorReading;
                        await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            this.txtGyroscope.Text = readings.AccelerationX.ToString() + " " + readings.AccelerationY.ToString() + " " + readings.AccelerationX.ToString();
                        });
                    }
                };
                await bandClient.SensorManager.Gyroscope.StartReadingsAsync();

                // Subscribe to HeartRate data.
                bandClient.SensorManager.HeartRate.ReadingChanged += async (s, args) =>
                {
                    samplesReceivedHR++;
                    IBandHeartRateReading readings = args.SensorReading;
                    stackHeartRate.Push(readings.HeartRate);
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        this.txtHR.Text = readings.HeartRate.ToString() + " [" + readings.Quality.ToString() + "]";
                    });

                };
                await bandClient.SensorManager.HeartRate.StartReadingsAsync();

                // Subscribe to Pedometer data.
                bandClient.SensorManager.Pedometer.ReadingChanged += async (s, args) =>
                {
                    samplesReceivedPed++;
                    IBandPedometerReading readings = args.SensorReading;
                    stackPedometer.Push(readings.TotalSteps);
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        this.txtPedometer.Text = readings.TotalSteps.ToString();
                    });
                };
                await bandClient.SensorManager.Pedometer.StartReadingsAsync();

                // Subscribe to SkinTemperature data.
                bandClient.SensorManager.SkinTemperature.ReadingChanged += async (s, args) =>
                {
                    samplesReceivedST++;
                    IBandSkinTemperatureReading readings = args.SensorReading;
                    stackSkinTemperature.Push(readings.Temperature);
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        this.txtSkinTemp.Text = readings.Temperature.ToString();
                    });
                };
                await bandClient.SensorManager.SkinTemperature.StartReadingsAsync();

                // Subscribe to UV data.
                bandClient.SensorManager.UV.ReadingChanged += async (s, args) =>
                {
                    samplesReceivedUV++;
                    IBandUVReading readings = args.SensorReading;
                    stackUV.Push(readings.IndexLevel.ToString());
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        this.txtUV.Text = readings.IndexLevel.ToString();
                    });
                };
                await bandClient.SensorManager.UV.StartReadingsAsync();
            }
            catch (Exception ex)
            {
                //this.StatusMessage.Text = ex.ToString();
            }

            this.ButtonRun.IsEnabled = false;
            this.ButtonStop.IsEnabled = true;
        }

        private async void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            // Stop the sensor subscriptions
            await bandClient.SensorManager.Accelerometer.StopReadingsAsync();
            await bandClient.SensorManager.Calories.StopReadingsAsync();
            await bandClient.SensorManager.Contact.StopReadingsAsync();
            await bandClient.SensorManager.Distance.StopReadingsAsync();
            await bandClient.SensorManager.Gyroscope.StopReadingsAsync();
            await bandClient.SensorManager.HeartRate.StopReadingsAsync();
            await bandClient.SensorManager.Pedometer.StopReadingsAsync();
            await bandClient.SensorManager.SkinTemperature.StopReadingsAsync();
            await bandClient.SensorManager.UV.StopReadingsAsync();

            this.ButtonRun.IsEnabled = true;
            this.ButtonStop.IsEnabled = false;
        }
    }
}
