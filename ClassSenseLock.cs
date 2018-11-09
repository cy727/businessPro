using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace business
{
    class ClassSenseLock
    {
        //Sense4 API

        // ctlCode definition for S4Control
        static public uint S4_LED_UP = 0x00000004;  // LED up
        static public uint S4_LED_DOWN = 0x00000008;  // LED down
        static public uint S4_LED_WINK = 0x00000028;  // LED wink
        static public uint S4_GET_DEVICE_TYPE = 0x00000025;	//get device type
        static public uint S4_GET_SERIAL_NUMBER = 0x00000026;	//get device serial
        static public uint S4_GET_VM_TYPE = 0x00000027;  // get VM type
        static public uint S4_GET_DEVICE_USABLE_SPACE = 0x00000029;  // get total space
        static public uint S4_SET_DEVICE_ID = 0x0000002a;  // set device ID

        // device type definition 
        static public uint S4_LOCAL_DEVICE = 0x00;		// local device 
        static public uint S4_MASTER_DEVICE = 0x80;		// net master device
        static public uint S4_SLAVE_DEVICE = 0xc0;		// net slave device

        // vm type definiton 
        static public uint S4_VM_51 = 0x00;		// VM51
        static public uint S4_VM_251_BINARY = 0x01;		// VM251 binary mode
        static public uint S4_VM_251_SOURCE = 0x02;		// VM251 source mode


        // PIN type definition 
        static public uint S4_USER_PIN = 0x000000a1;		// user PIN
        static public uint S4_DEV_PIN = 0x000000a2;		// dev PIN
        static public uint S4_AUTHEN_PIN = 0x000000a3;		// autheticate Key


        // file type definition 
        static public uint S4_RSA_PUBLIC_FILE = 0x00000006;		// RSA public file
        static public uint S4_RSA_PRIVATE_FILE = 0x00000007;		// RSA private file 
        static public uint S4_EXE_FILE = 0x00000008;		// VM file
        static public uint S4_DATA_FILE = 0x00000009;		// data file

        // dwFlag definition for S4WriteFile
        static public uint S4_CREATE_NEW = 0x000000a5;		// create new file
        static public uint S4_UPDATE_FILE = 0x000000a6;		// update file
        static public uint S4_KEY_GEN_RSA_FILE = 0x000000a7;		// produce RSA key pair
        static public uint S4_SET_LICENCES = 0x000000a8;		// set the license number for modle,available for net device only
        static public uint S4_CREATE_ROOT_DIR = 0x000000ab;		// create root directory, available for empty device only
        static public uint S4_CREATE_SUB_DIR = 0x000000ac;		// create child directory
        static public uint S4_CREATE_MODULE = 0x000000ad;		// create modle, available for net device only

        // the three parameters below must be bitwise-inclusive-or with S4_CREATE_NEW, only for executive file
        static public uint S4_FILE_READ_WRITE = 0x00000000;      // can be read and written in executive file,default
        static public uint S4_FILE_EXECUTE_ONLY = 0x00000100;      // can NOT be read or written in executive file
        static public uint S4_CREATE_PEDDING_FILE = 0x00002000;		// create padding file


        /* return value*/
        static public uint S4_SUCCESS = 0x00000000;		// succeed
        static public uint S4_UNPOWERED = 0x00000001;
        static public uint S4_INVALID_PARAMETER = 0x00000002;
        static public uint S4_COMM_ERROR = 0x00000003;
        static public uint S4_PROTOCOL_ERROR = 0x00000004;
        static public uint S4_DEVICE_BUSY = 0x00000005;
        static public uint S4_KEY_REMOVED = 0x00000006;
        static public uint S4_INSUFFICIENT_BUFFER = 0x00000011;
        static public uint S4_NO_LIST = 0x00000012;
        static public uint S4_GENERAL_ERROR = 0x00000013;
        static public uint S4_UNSUPPORTED = 0x00000014;
        static public uint S4_DEVICE_TYPE_MISMATCH = 0x00000020;
        static public uint S4_FILE_SIZE_CROSS_7FFF = 0x00000021;
        static public uint S4_DEVICE_UNSUPPORTED = 0x00006a81;
        static public uint S4_FILE_NOT_FOUND = 0x00006a82;
        static public uint S4_INSUFFICIENT_SECU_STATE = 0x00006982;
        static public uint S4_DIRECTORY_EXIST = 0x00006901;
        static public uint S4_FILE_EXIST = 0x00006a80;
        static public uint S4_INSUFFICIENT_SPACE = 0x00006a84;
        static public uint S4_OFFSET_BEYOND = 0x00006B00;
        static public uint S4_PIN_BLOCK = 0x00006983;
        static public uint S4_FILE_TYPE_MISMATCH = 0x00006981;
        static public uint S4_CRYPTO_KEY_NOT_FOUND = 0x00009403;
        static public uint S4_APPLICATION_TEMP_BLOCK = 0x00006985;
        static public uint S4_APPLICATION_PERM_BLOCK = 0x00009303;
        static public int S4_DATA_BUFFER_LENGTH_ERROR = 0x00006700;
        static public uint S4_CODE_RANGE = 0x00010000;
        static public uint S4_CODE_RESERVED_INST = 0x00020000;
        static public uint S4_CODE_RAM_RANGE = 0x00040000;
        static public uint S4_CODE_BIT_RANGE = 0x00080000;
        static public uint S4_CODE_SFR_RANGE = 0x00100000;
        static public uint S4_CODE_XRAM_RANGE = 0x00200000;
        static public uint S4_ERROR_UNKNOWN = 0xffffffff;

        //ADD
        static public uint S4_GET_LICENSE = 0x00000020;					/** get license */
        static public uint S4_FREE_LICENSE = 0x00000021;					/** free license */
        static public uint S4_MODIFY_TIMOUT = 0x00000022;					/** change the timeout*/

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SENSE4_CONTEXT
        {
            public int dwIndex;		//device index
            public int dwVersion;		//version		
            public int hLock;			//device handle
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public byte[] reserve;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 56)]
            public byte[] bAtr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] bID;
            public uint dwAtrLen;
        }

        //Assume that Sense4user.dll in c:\, if not, modify the lines below
        [DllImport(@"sense4user.dll")]
        private static extern uint S4Enum([MarshalAs(UnmanagedType.LPArray), Out] SENSE4_CONTEXT[] s4_context, ref uint size);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4Open(ref SENSE4_CONTEXT s4_context);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4Close(ref SENSE4_CONTEXT s4_context);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4Control(ref SENSE4_CONTEXT s4Ctx, uint ctlCode, byte[] inBuff,
            uint inBuffLen, byte[] outBuff, uint outBuffLen, ref uint BytesReturned);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4CreateDir(ref SENSE4_CONTEXT s4Ctx, string DirID, uint DirSize, uint Flags);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4ChangeDir(ref SENSE4_CONTEXT s4Ctx, string Path);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4EraseDir(ref SENSE4_CONTEXT s4Ctx, string DirID);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4VerifyPin(ref SENSE4_CONTEXT s4Ctx, byte[] Pin, uint PinLen, uint PinType);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4ChangePin(ref SENSE4_CONTEXT s4Ctx, byte[] OldPin, uint OldPinLen,
            byte[] NewPin, uint NewPinLen, uint PinType);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4WriteFile(ref SENSE4_CONTEXT s4Ctx, string FileID, uint Offset,
            byte[] Buffer, uint BufferSize, uint FileSize, ref uint BytesWritten, uint Flags,
            uint FileType);
        [DllImport(@"sense4user.dll")]
        private static extern uint S4Execute(ref SENSE4_CONTEXT s4Ctx, string FileID, byte[] InBuffer,
            uint InbufferSize, byte[] OutBuffer, uint OutBufferSize, ref uint BytesReturned);

        /// <summary>
        /// 
        /// </summary>
        /// 

        public SENSE4_CONTEXT[] si;
        public int checkSenseLock()
        {
            try
            {
                //enumerate devices			
                uint size = 0;
                uint BytesReturned = 0;
                byte[] wModID = new byte[1];
                wModID[0] = 0;
                uint ret = S4Enum(null, ref size);
                if (ret != S4_INSUFFICIENT_BUFFER)
                {
                    //Console.WriteLine("Enumerate EliteIV failed! <error code: {0,3:x}>", ret);
                    return -1; //-1读狗错误
                }

                si = new SENSE4_CONTEXT[size / Marshal.SizeOf(typeof(SENSE4_CONTEXT))];
                ret = S4Enum(si, ref size);

                if (ret != S4_SUCCESS)
                {
                    //Console.WriteLine("Enum failed! <error code: {0,3:x}>", ret);
                    return -1;
                }

                //open the first device
                ret = S4Open(ref si[0]);
                if (ret != S4_SUCCESS)
                {
                    //Console.WriteLine("S4Open for device failed with error! <error code: {0,3:x}>", ret);
                    return -1;
                }


                //get license
                ret = S4Control(ref si[0], S4_GET_LICENSE, wModID, 2, null, 0, ref BytesReturned);
                if (ret != S4_SUCCESS)
                {
                    //Console.WriteLine("Get license  failed! <error code: {0,3:x}>", ret);
                    return -2; //-2 权限错误
                }
                else
                {
                    //Console.WriteLine("Get license success");
                    //return -1;
                }

                byte[] test = { 1, 9, 6, 9, 0, 7, 2, 7, 2, 2 };
                //execute 0001
                ret = S4Execute(ref si[0], "c0c0", test, 10, test, 10, ref BytesReturned);
                if (ret != S4_SUCCESS)
                {
                    //Console.WriteLine("Execute  failed! <error code: {0,3:x}>", ret);
                    return -3; //执行错误
                }
                else
                {
                    if (BytesReturned != 2)
                        return -3;

                    if (test[0] != 1 || test[1] != 9 || test[2] != 6 || test[3] != 9)
                        return -3;

                }


                return 0;//成功
            }

            catch (Exception e)
            {
                return -4;//未知错误
            }

        }

        public int freeSenseLock()
        {
            try
            {
                //enumerate devices			
                uint BytesReturned = 0;
                uint ret;
                //free license
                ret = S4Control(ref si[0], S4_FREE_LICENSE, null, 0, null, 0, ref BytesReturned);
                if (ret != S4_SUCCESS)
                {
                    return -3; //执行错误
                    // Console.WriteLine("Free license  failed! <error code: {0,3:x}>", ret);
                }
                else
                {
                    //Console.WriteLine("Free license  success!");
                }

                //close device
                ret = S4Close(ref si[0]);
                if (ret != S4_SUCCESS)
                {
                    return -3; //执行错误
                    // Console.WriteLine("Close  failed! <error code: {0,3:x}>", ret);
                }
                return 0;//成功
            }

            catch (Exception e)
            {
                return -4;//未知错误
            }

        }

        /*
[STAThread]

public int ReadSenseLock()
{
    try
    {
        //enumerate devices			
        uint size = 0;
        uint BytesReturned = 0;
        byte[] wModID = new byte[1];
        wModID[0] = 0;
        uint ret = S4Enum(null, ref size);
        if (ret != S4_INSUFFICIENT_BUFFER)
        {
            //Console.WriteLine("Enumerate EliteIV failed! <error code: {0,3:x}>", ret);
            return -1; //-1读狗错误
        }

        SENSE4_CONTEXT[] si = new SENSE4_CONTEXT[size / Marshal.SizeOf(typeof(SENSE4_CONTEXT))];
        ret = S4Enum(si, ref size);

        if (ret != S4_SUCCESS)
        {
            //Console.WriteLine("Enum failed! <error code: {0,3:x}>", ret);
            return -1;
        }

        //open the first device
        ret = S4Open(ref si[0]);
        if (ret != S4_SUCCESS)
        {
            //Console.WriteLine("S4Open for device failed with error! <error code: {0,3:x}>", ret);
            return -1;
        }


        //get license
        ret = S4Control(ref si[0], S4_GET_LICENSE, wModID, 2, null, 0, ref BytesReturned);
        if (ret != S4_SUCCESS)
        {
            //Console.WriteLine("Get license  failed! <error code: {0,3:x}>", ret);
            return -2; //-2 权限错误
        }
        else
        {
            //Console.WriteLine("Get license success");
            //return -1;
        }

        byte[] test = { 1, 9, 6, 9, 0, 7, 2, 7, 2, 2 };
        //execute 0001
        ret = S4Execute(ref si[0], "c0c0", test, 10, test, 10, ref BytesReturned);
        if (ret != S4_SUCCESS)
        {
            //Console.WriteLine("Execute  failed! <error code: {0,3:x}>", ret);
            return -3; //执行错误
        }
        else
        {
            if (BytesReturned != 2)
                return -3;

            if (test[0] != 1 || test[1] != 9 || test[2] != 6 || test[3] != 9)
                return -3;

        }

        //free license
        ret = S4Control(ref si[0], S4_FREE_LICENSE, null, 0, null, 0, ref BytesReturned);
        if (ret != S4_SUCCESS)
        {
            return -3; //执行错误
            // Console.WriteLine("Free license  failed! <error code: {0,3:x}>", ret);
        }
        else
        {
            //Console.WriteLine("Free license  success!");
        }

        //close device
        ret = S4Close(ref si[0]);
        if (ret != S4_SUCCESS)
        {
            return -3; //执行错误
            // Console.WriteLine("Close  failed! <error code: {0,3:x}>", ret);
        }
        return 0;//成功
    }

    catch (Exception e)
    {
        return -4;//未知错误
    }

}
 */
    }
}
