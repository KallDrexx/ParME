using System;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Parme.MonoGame;

namespace Parme.Editor
{
    public class TextureFileLoader : ITextureFileLoader
    {
        /// <summary>
        /// How long we should wait after the latest change notification before attempting to load the texture
        /// </summary>
        private const int MinimumMsSinceLastChange = 300;
        
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ApplicationState _applicationState;
        private readonly FileSystemWatcher _fileSystemWatcher;
        private Texture2D _cachedTexture;
        private string _cachedTexturePath;
        private long _lastTextureChangedEventFiredAt;
        private long _cachedTextureLoadedAt;
        
        public TextureFileLoader(GraphicsDevice graphicsDevice, ApplicationState applicationState)
        {
            _graphicsDevice = graphicsDevice;
            _applicationState = applicationState;
            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.Changed += FileSystemWatcherOnChanged;
        }

        public bool CachedTextureHasPendingUpdate()
        {
            if (_cachedTexture == null)
            {
                return false;
            }
            
            var lastUpdatedAtTicks = _lastTextureChangedEventFiredAt;
            var lastUpdatedAt = new DateTime(lastUpdatedAtTicks);
            return lastUpdatedAtTicks > _cachedTextureLoadedAt &&
                   (DateTime.Now - lastUpdatedAt).TotalMilliseconds >= MinimumMsSinceLastChange;
        }

        public Texture2D LoadTexture2D(string path)
        {
            // All file names should be relative to the emitter
            var emitterPath = Path.GetDirectoryName(_applicationState.ActiveFileName);
            
            // NOTE: GetFullPath guarantees combined relative paths are properly absolute
            var fullPath = Path.GetFullPath(Path.Combine(emitterPath!, path));

            var isRequestingCachedTexture = _cachedTexture != null && fullPath == _cachedTexturePath;
            
            if (isRequestingCachedTexture && !CachedTextureHasPendingUpdate())
            {
                return _cachedTexture;
            }

            _cachedTexture = Texture2D.FromFile(_graphicsDevice, fullPath);
            _cachedTexturePath = fullPath;

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Path = Path.GetDirectoryName(fullPath);
            _fileSystemWatcher.Filter = Path.GetFileName(fullPath);
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            _fileSystemWatcher.EnableRaisingEvents = true;
            _cachedTextureLoadedAt = DateTime.Now.Ticks;
            
            return _cachedTexture;
        }

        private void FileSystemWatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            // We don't want to immediately load the texture that was updated, as this notification will trigger
            // multiple times while the file is still being modified.  So instead we just want to note down the time
            // this file changed event occurred, so we know that if we go more than a number of milliseconds after the
            // last change event the writing app should be done with its changes.
            Interlocked.Exchange(ref _lastTextureChangedEventFiredAt, DateTime.Now.Ticks);
        }
    }
}