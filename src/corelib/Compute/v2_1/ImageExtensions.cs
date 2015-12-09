﻿using System;
using OpenStack.Compute.v2_1;
using OpenStack.Synchronous.Extensions;

// ReSharper disable once CheckNamespace
namespace OpenStack.Synchronous
{
    /// <summary />
    public static class ImageExtensions_v2_1
    {
        /// <inheritdoc cref="ImageReference.GetImageAsync"/>
        public static Image GetImage(this ImageReference image)
        {
            return image.GetImageAsync().ForceSynchronous();
        }

        /// <inheritdoc cref="Image.WaitUntilActiveAsync"/>
        public static void WaitUntilActive(this Image image, TimeSpan? refreshDelay = null, TimeSpan? timeout = null, IProgress<bool> progress = null)
        {
            image.WaitUntilActiveAsync(refreshDelay, timeout, progress).ForceSynchronous();
        }

        /// <inheritdoc cref="ImageReference.DeleteAsync"/>
        public static void Delete(this ImageReference image)
        {
            image.DeleteAsync().ForceSynchronous();
        }

        /// <inheritdoc cref="Image.WaitUntilActiveAsync"/>
        public static void WaitUntilDeleted(this ImageReference image, TimeSpan? refreshDelay = null, TimeSpan? timeout = null, IProgress<bool> progress = null)
        {
            image.WaitUntilDeletedAsync(refreshDelay, timeout, progress).ForceSynchronous();
        }
    }
}
