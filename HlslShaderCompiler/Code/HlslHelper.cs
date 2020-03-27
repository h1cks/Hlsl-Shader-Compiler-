
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

        const string includeDirectory = "GPUCode\\";


        IDisposable m_shadow;

        public IDisposable Shadow
        {
            get
            {
                return m_shadow;
            }

            set
            {
                m_shadow = value;
            }
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
                if (File.Exists(includeDirectory + fileName))
                {
#if WINDOWS_UWP
                    DataStream a = GetFileStreamInclude(fileName);
#else
                    FileStream a = new FileStream(includeDirectory + fileName, FileMode.Open);
#endif
                    return a;
                }
            }
            catch (Exception ex)

            {
                ErrorHandler.DoErrorHandling(ex, ErrorHandler.GetCurrentMethod(ex));

            }

            throw new Exception("No file found: " + includeDirectory + fileName);

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
