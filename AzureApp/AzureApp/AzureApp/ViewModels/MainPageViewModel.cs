using AzureApp.Classes;
using AzureApp.Entities;
using AzureApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services.Media;

namespace AzureApp.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        private INavigation _navigation;
        private readonly TaskScheduler _scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        private IProcessProvider processProvider;
        private IMediaPicker _Mediapicker;

        private ImageSource unknown = Device.OnPlatform(
                        iOS: ImageSource.FromFile("unknown.jpg"),
                        Android: ImageSource.FromFile("unknown.jpg"),
                        WinPhone: ImageSource.FromFile("unknown.jpg"));

        private ImageSource m_Picture;
        private Stream m_PictureStream;
        private string m_Status = string.Empty;
        private string m_Fullname = string.Empty;
        private string m_Age = string.Empty;
        private string m_Weight = string.Empty;
        private List<string> m_Gender = new List<string>();
        private string m_SelectedGender = string.Empty;
        private bool m_IsRunning = false;
        private bool m_IsNextEnabled = true;
        private string m_EstimatedIntensity = string.Empty;

        public ICommand TakePhotoCommand { get; set; }
        public ICommand NextPageCommand { get; set; }

        public MainPageViewModel(INavigation navigation)
        {
            _navigation = navigation;
            Setup();

            TakePhotoCommand = new Command(async () =>
            {
                var isSimulator = DependencyService.Get<ISpecificPlatform>().CheckIfSimulator();
                if (isSimulator)
                {
                    Utilities.DisplayMessage("Action not supported, image will be loaded in simulator mode.");
                }else
                {
                    await TakePictureAsync();
                }
            });

            NextPageCommand = new Command(async () =>
            {
                var isSimulator = DependencyService.Get<ISpecificPlatform>().CheckIfSimulator();
                if (isSimulator)
                    PictureStream = processProvider.GetDummyStream();

                if (string.IsNullOrEmpty(Fullname) || string.IsNullOrEmpty(Age) || string.IsNullOrEmpty(Weight) || SelectedGender == "Select a gender" || PictureStream == null)
                {
                    Utilities.DisplayMessage("Please ensure all required fields are filled.");
                }
                else
                {
                    IsRunning = true;
                    IsNextEnabled = false;

                    Participant participant = new Participant();
                    participant.FullName = Fullname;
                    participant.Age = Age;
                    participant.Weight = Weight;
                    participant.Gender = SelectedGender;

                    //save participant in cloud.
                    await processProvider.ProcessParticipantAsync(PictureStream)
                    .ContinueWith(async (b) =>
                    {
                        if (b.Result == string.Empty)
                        {
                            IsRunning = false;
                            IsNextEnabled = true;

                            Utilities.DisplayMessage("There was a problem with the blob service.");
                        }
                        else
                        {
                            participant.FileName = b.Result;

                            //save record in cloud.
                            await Sender.SaveDataInCloudAsync(participant)
                            .ContinueWith((c) =>
                            {
                                if (c.Result)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        IsRunning = false;
                                        IsNextEnabled = true;

                                        Utilities.DisplayMessage("Record saved successfully.");
                                    });
                                }
                                else
                                {
                                    IsRunning = false;
                                    IsNextEnabled = true;

                                    Utilities.DisplayMessage("There was a problem with the service.");
                                }
                            });
                        }
                    });
                }
            });
        }

        private void Setup()
        {
            m_Gender.Add("Select a gender");
            m_Gender.Add("Male");
            m_Gender.Add("Female");
            SelectedGender = Gender[0];

            this.processProvider = DependencyService.Get<IProcessProvider>();

            if (_Mediapicker == null)
            {
                try
                {
                    var device = Resolver.Resolve<IDevice>();
                    _Mediapicker = DependencyService.Get<IMediaPicker>() ?? device.MediaPicker;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error: {0}", ex.Message);
                }
            }
        }

        public async Task<MediaFile> TakePictureAsync()
        {
            Picture = null;

            return await _Mediapicker.TakePhotoAsync(new CameraMediaStorageOptions
            {
                DefaultCamera = CameraDevice.Front,
                MaxPixelDimension = 400
            }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Status = t.Exception.InnerException.ToString();
                }
                else if (t.IsCanceled)
                {
                    Status = "Canceled";
                }
                else
                {
                    var mediaFile = t.Result;
                    Picture = ImageSource.FromStream(() => mediaFile.Source);
                    PictureStream = mediaFile.Source;

                    return mediaFile;
                }

                return null;
            }, _scheduler);
        }

        public Stream PictureStream
        {
            get
            {
                return m_PictureStream;
            }
            set
            {
                m_PictureStream = value;
                OnPropertyChanged();
            }
        }

        public ImageSource Picture
        {
            get
            {
                if (m_Picture == null)
                {
                    m_Picture = unknown;
                }
                return m_Picture;
            }
            set
            {
                m_Picture = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get
            {
                return m_Status;
            }
            set
            {
                m_Status = value;
                OnPropertyChanged();
            }
        }

        public string Fullname
        {
            get
            {
                return m_Fullname;
            }
            set
            {
                m_Fullname = value;
                OnPropertyChanged();
            }
        }

        public string Age
        {
            get
            {
                return m_Age;
            }
            set
            {
                m_Age = value;
                OnPropertyChanged();
            }
        }

        public string Weight
        {
            get
            {
                return m_Weight;
            }
            set
            {
                m_Weight = value;
                OnPropertyChanged();
            }
        }

        public List<string> Gender
        {
            get
            {
                return m_Gender;
            }
        }

        public string SelectedGender
        {
            get
            {
                return m_SelectedGender;
            }
            set
            {
                m_SelectedGender = value;
                OnPropertyChanged();
            }
        }

        public bool IsRunning
        {
            get
            {
                return m_IsRunning;
            }
            set
            {
                m_IsRunning = value;
                OnPropertyChanged();
            }
        }

        public bool IsNextEnabled
        {
            get
            {
                return m_IsNextEnabled;
            }
            set
            {
                m_IsNextEnabled = value;
                OnPropertyChanged();
            }
        }

        public string EstimatedIntensity
        {
            get
            {
                return m_EstimatedIntensity;
            }
            set
            {
                m_EstimatedIntensity = value;
                OnPropertyChanged();
            }
        }
    }
}