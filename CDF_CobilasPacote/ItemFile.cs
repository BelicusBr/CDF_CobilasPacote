using System;

namespace Cobilas.IO.CobilasPackage {
    [Serializable]
    public sealed class ItemFile : IDisposable {
        private string relativePath;
        private byte[] content;
        private bool disposed;

        public byte[] Content => content;
        public bool Disposed => disposed;
        public string RelativePath => relativePath;
        public int Count => content == null ? 0 : content.Length;

        internal ItemFile(string relativePath, byte[] content) {
            this.relativePath = relativePath;
            this.content = content;
        }

        public void Rename(string relativePath)
            => this.relativePath = relativePath;

        public void ChangeContent(byte[] content) {
            Array.Clear(this.content, 0, Count);
            this.content = content;
        }

        public void Dispose() {
            if (disposed) return;
            disposed = true;
            relativePath = null;
            if (content != null)
                if (content.Length != 0) {
                    Array.Clear(content, 0, content.Length);
                    content = null;
                }
        }
    }
}
