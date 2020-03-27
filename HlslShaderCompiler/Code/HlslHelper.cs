
using SharpDX;
using SharpDX.D3DCompiler;
using System;
using System.IO;


#if WINDOWS_UWP
using Windows.Storage;
#endif

namespace Insane3D.HelperFunctions
{
    public class IncludeFX : Include
    {
        string _includeDirectory = "";

        protected IDisposable _shadow;

        public IDisposable Shadow
        {
            get
            {
                return _shadow;
            }

            set
            {
                _shadow = value;
            }
        }

        public IncludeFX(string directory)
        {
            _includeDirectory = directory;
        }

        public void Close(Stream stream)
        {
            stream.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            try
            {
                if (File.Exists(_includeDirectory + fileName))
                {
#if WINDOWS_UWP
                    DataStream a = GetFileStreamInclude(fileName);
#else
                    FileStream stream_ = new FileStream(_includeDirectory + fileName, FileMode.Open);
#endif
                    return stream_;
                }
            }
            catch (Exception ex)

            {
                ErrorHandler.DoErrorHandling(ex, ErrorHandler.GetCurrentMethod(ex));

            }

            throw new Exception("No file found: " + _includeDirectory + fileName);

        }

#if WINDOWS_UWP
        DataStream GetFileStreamInclude(string fileName)
        {
            var file = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///GPUCode/" + fileName));
            file.AsTask().Wait();

            var content = file.GetResults().OpenReadAsync();
            {
                content.AsTask().Wait();
                var c = content.GetResults();
                DataStream a = new DataStream((int)c.Size, true, true);

                c.AsStream().CopyTo(a);
                a.Position = 0;

               c.Dispose();

                return (DataStream)a;
            }


            //content.Dispose();


            //  return null;
        }

        DataStream GetFileStreamUWP(string fileName, string a_filePath)
        {
            var file = StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + a_filePath + fileName));
            file.AsTask().Wait();

            var content = file.GetResults().OpenReadAsync();
            {
                content.AsTask().Wait();
                var c = content.GetResults();
                DataStream a = new DataStream((int)c.Size, true, true);

                c.AsStream().CopyTo(a);
                a.Position = 0;

                return (DataStream)a;
            }


            //content.Dispose();


            //  return null;
        }

#endif

        protected virtual void Dispose(bool Disposing)
        {
            if (Disposing)
            {
            }
        }


    }
}
