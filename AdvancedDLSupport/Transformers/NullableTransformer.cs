﻿using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace AdvancedDLSupport
{
    /// <summary>
    /// Raises or lowers nullable value types to pointers.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    [PublicAPI]
    public class NullableTransformer<T> : PointerTransformer<T?> where T : struct
    {
        /// <inheritdoc />
        public override IntPtr LowerValue(T? value)
        {
            if (!value.HasValue)
            {
                return IntPtr.Zero;
            }

            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr(value.Value, ptr, false);

            return ptr;
        }

        /// <inheritdoc />
        public override T? RaiseValue(IntPtr value)
        {
            if (value == IntPtr.Zero)
            {
                return null;
            }

            var val = Marshal.PtrToStructure<T>(value);
            return val;
        }
    }
}
