﻿using System;
using System.Extensions;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenStack.Serialization;

namespace OpenStack.Compute.v2_1
{
    /// <summary />
    [JsonConverterWithConstructor(typeof(RootWrapperConverter), "image")]
    public class Image : ImageReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
        {
            Metadata = new ImageMetadata();
        }

        /// <summary />
        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        /// <summary />
        [JsonProperty("updated")]
        public DateTimeOffset LastModified { get; set; }

        /// <summary />
        [JsonProperty("minDisk")]
        public int MinimumDiskSize { get; set; }

        /// <summary />
        [JsonProperty("minRam")]
        public int MinimumMemorySize { get; set; }

        /// <summary />
        [JsonProperty("OS-EXT-IMG-SIZE:size")]
        public int? Size { get; set; }

        /// <summary />
        [JsonProperty("progress")]
        public int Progress { get; set; }

        /// <summary />
        [JsonProperty("status")]
        public ImageStatus Status { get; set; }

        /// <summary />
        [JsonProperty("server")]
        public ServerReference Server { get; set; }

        /// <summary />
        [JsonProperty("metadata")]
        public ImageMetadata Metadata { get; set; }

        /// <summary />
        [JsonIgnore]
        public ImageType Type
        {
            get
            {
                string type;
                if (Metadata != null && Metadata.TryGetValue("image_type", out type))
                    return StringEnumeration.FromDisplayName<ImageType>(type);

                return ImageType.Base;
            }
        }

        /// <inheritdoc cref="ComputeApiBuilder.WaitForImageStatusAsync{TImage,TStatus}" />
        /// <exception cref="InvalidOperationException">When this instance was not constructed by the <see cref="ComputeService"/>, as it is missing the appropriate internal state to execute service calls.</exception>
        public async Task WaitForStatus(ImageStatus status, TimeSpan? refreshDelay = null, TimeSpan? timeout = null, IProgress<bool> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var owner = this.TryGetOwner<ComputeApiBuilder>();
            var result = await owner.WaitForImageStatusAsync<Image, ImageStatus>(Id, status, refreshDelay, timeout, progress, cancellationToken).ConfigureAwait(false);
            result.CopyProperties(this);
        }

        /// <exception cref="InvalidOperationException">When this instance was not constructed by the <see cref="ComputeService"/>, as it is missing the appropriate internal state to execute service calls.</exception>
        public Task WaitUntilActiveAsync(TimeSpan? refreshDelay = null, TimeSpan? timeout = null, IProgress<bool> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return WaitForStatus(ImageStatus.Active, refreshDelay, timeout, progress, cancellationToken);
        }
        
        /// <inheritdoc cref="ComputeApiBuilder.WaitUntilImageIsDeletedAsync{TImage,TStatus}" />
        /// <exception cref="InvalidOperationException">When this instance was not constructed by the <see cref="ComputeService"/>, as it is missing the appropriate internal state to execute service calls.</exception>
        public override async Task WaitUntilDeletedAsync(TimeSpan? refreshDelay = null, TimeSpan? timeout = null, IProgress<bool> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.WaitUntilDeletedAsync(refreshDelay, timeout, progress, cancellationToken).ConfigureAwait(false);
            Status = ImageStatus.Deleted;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Metadata.Image = this;
        }
    }
}
