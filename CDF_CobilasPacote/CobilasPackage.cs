using System;
using System.Collections.Generic;

namespace Cobilas.IO.CobilasPackage {
    [Serializable]
    public class CobilasPackage : IDisposable {
        private string name;
        private List<ItemFile> items;
        private bool disposed;

        public string Name => name;
        public bool Disposed => disposed;
        public int Count => items.Count;

        public ItemFile this[int index] => items[index];

        public ItemFile this[string relativePath] => this[IndexOf(relativePath)];

        public CobilasPackage(string name) {
            this.name = name;
            items = new List<ItemFile>();
        }

        public void Rename(string name)
            => this.name = name;

        public void Add(ItemFile itemFile)
            => items.Add(itemFile);

        public void Add(string relativePath, byte[] content)
            => Add(new ItemFile(relativePath, content));

        public bool Contains(ItemFile itemFile)
            => items.Contains(itemFile);

        public bool Contains(string relativePath) {
            foreach (var item in items)
                if (item.RelativePath == relativePath)
                    return true;
            return false;
        }

        public void Remove(int index) {
            this[index].Dispose();
            items.RemoveAt(index);
        }

        public void Remove(string relativePath)
            => Remove(IndexOf(relativePath));

        public int IndexOf(string relativePath) {
            for (int I = 0; I < Count; I++)
                if (this[I].RelativePath == relativePath)
                    return I;
            return -1;
        }

        public void Dispose() {
            if (disposed) return;
            disposed = true;
            name = null;
            if (items != null) {
                items.ForEach((f) => f.Dispose());
                items.Clear();
                items.Capacity = 0;
                items = null;
            }
        }
    }
}
