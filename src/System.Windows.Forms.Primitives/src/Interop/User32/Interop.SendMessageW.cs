// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class User32
    {
        [DllImport(Libraries.User32, ExactSpelling = true)]
        public static extern IntPtr SendMessageW( //1
            IntPtr hWnd,
            WM Msg,
            IntPtr wParam = default,
            IntPtr lParam = default);

        public static IntPtr SendMessageW( //2
            HandleRef hWnd,
            WM Msg,
            IntPtr wParam = default,
            IntPtr lParam = default)
        {
            IntPtr result = SendMessageW(hWnd.Handle, Msg, wParam, lParam);
            GC.KeepAlive(hWnd.Wrapper);
            return result;
        }

        public static IntPtr SendMessageW( //3
            IHandle hWnd,
            WM Msg,
            IntPtr wParam = default,
            IntPtr lParam = default)
        {
            IntPtr result = SendMessageW(hWnd.Handle, Msg, wParam, lParam);
            GC.KeepAlive(hWnd);
            return result;
        }

        public unsafe static IntPtr SendMessageW( //4
            IntPtr hWnd,
            WM Msg,
            IntPtr wParam,
            string lParam)
        {
            fixed (char* c = lParam)
            {
                return SendMessageW(hWnd, Msg, wParam, (IntPtr)(void*)c);
            }
        }

        public unsafe static IntPtr SendMessageW( //5
            HandleRef hWnd,
            WM Msg,
            IntPtr wParam,
            string lParam)
        {
            fixed (char* c = lParam)
            {
                return SendMessageW(hWnd, Msg, wParam, (IntPtr)(void*)c);
            }
        }

        public unsafe static IntPtr SendMessageW( //6
            IHandle hWnd,
            WM Msg,
            IntPtr wParam,
            string lParam)
        {
            fixed (char* c = lParam)
            {
                return SendMessageW(hWnd, Msg, wParam, (IntPtr)(void*)c);
            }
        }

        public unsafe static IntPtr SendMessageW<T>( //7
            IntPtr hWnd,
            WM Msg,
            IntPtr wParam,
            ref T lParam) where T : unmanaged
        {
            fixed (void* l = &lParam)
            {
                return SendMessageW(hWnd, Msg, wParam, (IntPtr)l);
            }
        }

        public unsafe static IntPtr SendMessageW<T>( //8
            IHandle hWnd,
            WM Msg,
            IntPtr wParam,
            ref T lParam) where T : unmanaged
        {
            fixed (void* l = &lParam)
            {
                return SendMessageW(hWnd, Msg, wParam, (IntPtr)l);
            }
        }
    }
}
