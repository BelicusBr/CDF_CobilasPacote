using System;

namespace Cobilas.IO.CobilasPackage.CLI {
    internal struct KeyPaths {
        public string CommandType;
        public KeyPaths[] cellars;
        public Delegate action;

        public KeyPaths this[string CommandType] {
            get {
                if (cellars != null)
                    foreach (var item in cellars)
                        if (item.IsKey(CommandType))
                            return item;
                return new KeyPaths();
            }
        }

        public KeyPaths(string CommandType, Delegate action, KeyPaths[] cellars) {
            this.CommandType = CommandType;
            this.action = action;
            this.cellars = cellars;
        }

        public KeyPaths(string CommandType, Delegate action) :
            this(CommandType, action, (KeyPaths[])null) { }

        public KeyPaths(string CommandType, KeyPaths[] cellars) :
            this(CommandType, (Delegate)null, cellars) { }

        public KeyPaths(string CommandType) :
            this(CommandType, (Delegate)null) { }

        public bool HasAction()
            => action != null;

        public bool IsKey(string CommandType) {
            foreach (var item in this.CommandType.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
                if (item == CommandType)
                    return true;
            return false;
        }
    }
}
