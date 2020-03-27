
using D3D11 = SharpDX.Direct3D11;
using DXGI = SharpDX.DXGI;
using SharpDX;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System;
using SharpDX.WIC;
using System.Runtime.CompilerServices;
#if WINDOWS_UWP
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Storage;
#else
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;
#endif

namespace Insane3D.HelperFunctions
{

    public static class Helper
    {
        static readonly Random randGen = new Random();
        public const uint UnassignedUID = 0;
        static public float s_internalTime = 1000.0f + randGen.Next() % 500;

        static readonly Dictionary<int, Vector2[]> _textureDivision = new Dictionary<int, Vector2[]>();

        //==========================================================================================================================//

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(T value, T max, T min)
 where T : System.IComparable<T>
        {
            Debug.Assert(max.CompareTo(min) >= 0);

            T result = value;
            if (value.CompareTo(max) > 0)
            {
                result = max;
            }
            else if (value.CompareTo(min) < 0)
            {
                result = min;
            }

            return result;
        }

        //==========================================================================================================================//

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateRandomVector(in Vector3 planeRestriction, out Vector3 value)
        {
            do
            {
                int valx_ = Helper.NextRandom(2000) - 1000;
                int valy_ = Helper.NextRandom(2000) - 1000;
                int valz_ = Helper.NextRandom(2000) - 1000;
                value = new Vector3((float)valx_, (float)valy_, (float)valz_) * planeRestriction;
            }
            while (value.LengthSquared() == 0);

            value.Normalize();
        }

        //==========================================================================================================================//

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T subjectA, ref T subjectB)
        {
            T interrmittent = subjectA;

            subjectA = subjectB;

            subjectB = interrmittent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        //==========================================================================================================================//

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PageClamp<T>(T value, T max, T min)
where T : System.IComparable<T>
        {
            Debug.Assert(max.CompareTo(min) >= 0);

            T result = value;
            if (value.CompareTo(max) > 0)
            {
                result = min;
            }
            else if (value.CompareTo(min) < 0)
            {
                result = max;
            }

            return result;
        }

        //==========================================================================================================================//

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PageContain(float value, float max, float min, float mod)
        {
            Debug.Assert(max.CompareTo(min) >= 0);

            float result = value;
            if (value.CompareTo(max) > 0)
            {
                result = value - mod;
            }
            else if (value.CompareTo(min) < 0)
            {
                result = value + mod;
            }

            return result;
        }

        //==========================================================================================================================//

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Lerp(float valueA, float valueB, float amount)
        {
            return valueA + (valueB - valueA) * amount;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Lerp(int valueA, int valueB, float amount)
        {
            return valueA + (int)((valueB - valueA) * amount);
        }

        //==========================================================================================================================//

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DisposeObject<T>(ref T genericObject)
        {
            if (genericObject != null)
            {
                if (genericObject is IDisposable)
                {
                    (genericObject as IDisposable).Dispose();
                }
            }

            genericObject = default;
        }

        //==========================================================================================================================//

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetApplicationPath()
        {
#if WINDOWS_UWP
            
            return System.AppContext.BaseDirectory;
#else

            return Application.ExecutablePath;
#endif
        }

        //==========================================================================================================================//

        public static List<int> StringToIntList(string str)
        {
            List<int> TagIds = str.Split(',').Select(int.Parse).ToList();

            return TagIds;
        }

        public static List<bool> StringToBoolList(string str)
        {
            List<bool> TagIds = str.Split(',').Select(bool.Parse).ToList();

            return TagIds;
        }

        //==========================================================================================================================//


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetVersion()
        {
#if !WINDOWS_UWP
            string version;
#if DEBUG
            version = " Debug";
#else
            version = " Release";
#endif
#endif

#if WINDOWS_UWP
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion aversion = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", aversion.Major, aversion.Minor, aversion.Build, aversion.Revision);

           
#else
            return System.Reflection.Assembly.GetExecutingAssembly()
                                     .GetName()
                                     .Version
                                     .ToString() + version;
#endif

        }

        //==========================================================================================================================//

        public static void SaveTextureAsBitmap(D3D11.Device1 d3dDevice, D3D11.Texture2D texture2d, string fileName)
        {
            using (DataStream stream = new DataStream(texture2d.Description.Width * texture2d.Description.Height * 4, true, true))
            {
                Helper.ConvertTextureToBitmap(texture2d, stream, d3dDevice, out Bitmap bitmap);

                using (var fileStream = File.OpenWrite(fileName))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }

                bitmap.Dispose();
            }
        }

        //==========================================================================================================================//

        public static float GetAngle(Vector2 origin, Vector2 destination, float speed, float gravity)
        {
            //Labeling variables to match formula
            float x = Math.Abs(destination.X - origin.X);
            float y = Math.Abs(destination.Y - origin.Y);
            float v = speed;
            float g = gravity;

            //Formula seen above
            double valueToBeSquareRooted = Math.Pow(v, 4) - g * (g * Math.Pow(x, 2) + 2 * y * Math.Pow(v, 2));
            if (valueToBeSquareRooted >= 0)
            {
                return (float)Math.Atan((Math.Pow(v, 2) + Math.Sqrt(valueToBeSquareRooted)) / g * x);
            }
            else
            {
                return -1;
                //Destination is out of range
            }
        }

        //==========================================================================================================================//

        internal static string PathAddBackSlash(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            path = path.TrimEnd();

            if (PathEndsWithDirectorySeparator())
            {
                return path;
            }

            return path + GetDirectorySeparatorUsedInPath();

            bool PathEndsWithDirectorySeparator()
            {
                if (path.Length == 0)
                {
                    return false;
                }

                char lastChar = path[path.Length - 1];
                return lastChar == Path.DirectorySeparatorChar
                    || lastChar == Path.AltDirectorySeparatorChar;
            }

            char GetDirectorySeparatorUsedInPath()
            {
                if (path.Contains(Path.AltDirectorySeparatorChar))
                {
                    return Path.AltDirectorySeparatorChar;
                }

                return Path.DirectorySeparatorChar;
            }
        }

        //==========================================================================================================================//

        internal static float CalculateFireAngle(float speed, float range, float altitude, float gravityValue)
        {
            //    var angle = GetAngle(new Vector2(0, 0), new Vector2(range, altitude), speed, gravityValue);

            float b = speed * speed - altitude * gravityValue;
            float discriminant = b * b - gravityValue * gravityValue * (range * range + altitude * altitude);

            if (discriminant < 0)
            {
                return -1; // Out of range, need higher shot velocity.
            }

            float discRoot = (float)Math.Sqrt(discriminant);

            // Impact time for the most direct shot that hits.
            float T_min = (float)Math.Sqrt((b - discRoot) * 2 / (gravityValue * gravityValue));

            float actualDistance = range / speed;

            float ratio = T_min / actualDistance;

            float angle = (float)Math.Atan(ratio);

            return angle;
        }

        //==========================================================================================================================//

        public static void ConvertTextureToBitmap(this D3D11.Texture2D texture, DataStream stream, D3D11.Device1 deviceManager, out Bitmap outputBitmap)
        {
            //if (imgfactory == null)

            ImagingFactory imgfactory_ = new SharpDX.WIC.ImagingFactory();

            var textureCopy = new D3D11.Texture2D(deviceManager, new D3D11.Texture2DDescription
            {
                Width = (int)texture.Description.Width,
                Height = (int)texture.Description.Height,
                MipLevels = texture.Description.MipLevels,
                ArraySize = texture.Description.ArraySize,
                Format = texture.Description.Format,
                Usage = D3D11.ResourceUsage.Staging,
                SampleDescription = new DXGI.SampleDescription(1, 0),
                BindFlags = D3D11.BindFlags.None,
                CpuAccessFlags = D3D11.CpuAccessFlags.Read,
                OptionFlags = D3D11.ResourceOptionFlags.None
            });
            deviceManager.ImmediateContext.CopyResource(texture, textureCopy);

            var dataBox = deviceManager.ImmediateContext.MapSubresource(
                textureCopy,
                0,
                0,
                D3D11.MapMode.Read,
                SharpDX.Direct3D11.MapFlags.None,
                out DataStream dataStream);

            var dataRectangle = new DataRectangle
            {
                DataPointer = dataStream.DataPointer,
                Pitch = dataBox.RowPitch
            };

            outputBitmap = new Bitmap(
                imgfactory_,
                textureCopy.Description.Width,
                textureCopy.Description.Height,
                PixelFormat.Format32bppRGBA,
                dataRectangle);


            stream.Position = 0;
            using (var bitmapEncoder = new PngBitmapEncoder(imgfactory_, stream))
            {
                using (var bitmapFrameEncode = new BitmapFrameEncode(bitmapEncoder))
                {
                    bitmapFrameEncode.Initialize();
                    bitmapFrameEncode.SetSize(outputBitmap.Size.Width, outputBitmap.Size.Height);
                    var pixelFormat = PixelFormat.Format32bppRGBA;
                    bitmapFrameEncode.SetPixelFormat(ref pixelFormat);
                    bitmapFrameEncode.WriteSource(outputBitmap);
                    bitmapFrameEncode.Commit();
                    bitmapEncoder.Commit();
                }
            }

            deviceManager.ImmediateContext.UnmapSubresource(textureCopy, 0);
            textureCopy.Dispose();
            imgfactory_.Dispose();
        }



        //====================================================================================================================================//

        public static string GetRelativePath(string path)
        {
            string currentDir_ = Path.GetDirectoryName(GetApplicationPath()) + "\\";
            DirectoryInfo directory_ = new DirectoryInfo(currentDir_);
            DirectoryInfo directoryTarget_ = new DirectoryInfo(path);

            string exePath = directory_.FullName;
            string filePath = directoryTarget_.FullName;

            if (exePath.Length < filePath.Length)
            {
                if ((filePath.Substring(0, exePath.Length) == exePath))
                {
                    return filePath.Substring(exePath.Length);
                }
            }
            return path;

        }

        //====================================================================================================================================//

        

        static volatile uint _UIDCounter = 1;

        //====================================================================================================================================//

        public static uint GetUintUID()
        {
            return _UIDCounter++;
        }

        //====================================================================================================================================//

        public static string GetUniqueKey(int length)
        {
            string guidResult_ = string.Empty;

            do
            {
                // Get the GUID.
                guidResult_ += Guid.NewGuid().ToString().Replace("-","");
            }
            while (guidResult_.Length < length) ;

#if DEBUG
            // Make sure length is valid.
            if (length <= 0 || length > guidResult_.Length)
            {
                throw new ArgumentException("Length must be between 1 and " + guidResult_.Length);
            }
#endif
            // Return the first length bytes.
            return guidResult_.Substring(0, length);
        }

        //====================================================================================================================================//

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RestrictCoordinates(int x, int terrainDimension)
        {
            return Clamp(x, terrainDimension - 1, 0);
        }

        public static void ExitApplication()
        {
#if WINDOWS_UWP
            CoreApplication.Exit();
#else
            Application.Exit();
#endif
        }

        //====================================================================================================================================//
        public static void WriteMatrix(BinaryWriter binaryWriter, in Matrix instance)
        {
            float[] array_ = instance.ToArray();

            Debug.Assert(array_.Length == 16);

            for (int l = 0; l < array_.Length; l++)
            {
                binaryWriter.Write(array_[l]);
            }
        }

        //====================================================================================================================================//

        public static Matrix ReadMatrix(BinaryReader binaryreader)
        {
            float[] array_ = new float[16];

            for (int l = 0; l < array_.Length; l++)
            {
                array_[l] = binaryreader.ReadSingle();
            }

            return new Matrix(array_);
        }

        //====================================================================================================================================//

        static public Vector2 ReturnTextureUV(int frameindex, int division)
        {
            Vector2[] array_ = ReturnTextureUVArray(division);

            return array_[frameindex];
        }

        //====================================================================================================================================//

        static public Vector2[] ReturnTextureUVArray(int divisionSize)
        {
            float division;

            if (divisionSize < 1)
            {
                division = 1;
            }
            else
            {
                division = divisionSize;
            }

            if (!_textureDivision.TryGetValue(divisionSize, out Vector2[] array))
            {
                if (division > 1)
                {
                    array = new Vector2[divisionSize * divisionSize];
                    float divx, divy;

                    for (int y = 0; y < divisionSize; y++)
                    {
                        divy = y / division;
                        for (int x = 0; x < divisionSize; x++)
                        {
                            divx = x / division;
                            array[(y * divisionSize) + x] = new Vector2(divx, divy);
                        }
                    }
                }
                else
                {
                    array = new Vector2[1];
                    array[0] = new Vector2(0, 0);
                }

                _textureDivision.Add(divisionSize, array);
            }

            return array;
        }


        //=========================================================================================//

        static public void Load24BitBitmapInto32BitTexture(byte[] bits, DataStream buffer)
        {
            for (int i = 0; i < bits.Length; i += 3)
            {
                buffer.WriteByte(bits[i]);
                buffer.WriteByte(bits[i + 1]);
                buffer.WriteByte(bits[i + 2]);
                // if i % 3 = 2 then add an alpha channel byte
                if (bits[i] > 0)
                {
                    buffer.WriteByte(bits[i]); //need this to convert to full 32 bit
                }
                else
                {
                    buffer.WriteByte(0); //need this to convert to full 32 bit
                }
            }

            buffer.Position = 0;
        }

        //=========================================================================================//

        static public void Load32BitBitmapInto32BitTexture(byte[] bits, DataStream buffer)
        {
            for (int i = 0; i < bits.Length; i += 4)
            {
                buffer.WriteByte(bits[i]);
                buffer.WriteByte(bits[i + 1]);
                buffer.WriteByte(bits[i + 2]);
                buffer.WriteByte(bits[i + 3]);
            }

            buffer.Position = 0;
        }

        //=========================================================================================//

        public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Collection is null");
            }

            foreach (var item in collection)
            {
                if (!source.ContainsKey(item.Key))
                {
                    source.Add(item.Key, item.Value);
                }
                else
                {
                    // handle duplicate key issue here
                }
            }
        }

        //=========================================================================================//

        static public Stream GetFileStream(string filename, string filePath, FileMode fileMode = FileMode.Open)
        {
            return GetFileStream(filePath + filename, fileMode);
        }

        static public Stream GetFileStream(string filename, FileMode fileMode = FileMode.Open)
        {
            Stream data;

            if (!File.Exists(filename))
                Console.WriteLine("File does not exit: " +  filename);

#if !WINDOWS_UWP
            //m_fileStream = new SoundStream(File.OpenRead(filePath + filename));
            data = new FileStream(filename, fileMode);
#else
            data = GetFileStreamUWP(filename, filePath);
#endif

            return data;
        }

        //====================================================================================================================================//



        public static int NextRandom(int range, int lower = 0)
        {
            if (range > 0)
            {
                return lower + randGen.Next() % range;
            }
            else
            {
                return 0;
            }
        }

        //====================================================================================================================================//

        public static float NextRandomFloat(float randomRange, float lower = 0.0f)
        {
            if (randomRange > 0.0f)
            {
                const float milliseconds = 1000;
                const float divisorMilli = 1.0f / 1000.0f;
                int range = (int)(milliseconds * randomRange);
                float value = randGen.Next() % range;
                value *= divisorMilli;

                return lower + value;
            }

            return lower;
        }

        //====================================================================================================================================//

        
        public static float InternalTimer
        {
            get { return s_internalTime; }
            set { s_internalTime = value; }
        }

        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            GetWindowThreadProcessId(activatedHandle, out int activeProcId);

            return activeProcId == procId;
        }

        //====================================================================================================================================//

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

#if WINDOWS_UWP
  
        


        public static DataStream GetFileStreamUWP(string fileName, string a_filePath)
        {
        //    a_filePath = a_filePath.Replace("\\", "//");
        try
            { 
            var uri = new Uri("ms-appx:///" + a_filePath + fileName);

    

            var file = StorageFile.GetFileFromApplicationUriAsync(uri);
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

        }
            catch (Exception ex)
            {
                ErrorHandler.DoErrorHandling(ex, ErrorHandler.GetCurrentMethod(ex));

                return null;
            }
    //content.Dispose();


    //  return null;
}

#endif
    }



}