using System;
using System.Collections.Generic;
namespace Sloot {
    public class WeightSorter<T> {
        public enum SortMode {
            FIRST,
            LAST
        }
        SortMode sortMode = SortMode.FIRST;

        List<WeightInstance<T>> sorter = new List<WeightInstance<T>>();

        public event Action Update;

        public int Count { get { return sorter.Count; } }

        public T this[int index] {
            get {
                return sorter[index].GetInstance();
            }
        }

        public WeightSorter(SortMode sortMode) {
            this.sortMode = sortMode;
        }

        public WeightSorter() { }

        public void NewSortMode(SortMode sortMode) {
            this.sortMode = sortMode;
        }
        public void Add(WeightInstance<T> toAdd) {
            bool add = false;
            if (sortMode == SortMode.FIRST) {
                for (int i = 0; i < sorter.Count; i++) {
                    if (sorter[i].GetWeight() < toAdd.GetWeight()) {
                        sorter.Insert(i, toAdd);
                        add = true;
                        break;
                    }
                }
            } else if (sortMode == SortMode.LAST) {
                for (int i = 0; i < sorter.Count; i++) {
                    if (sorter[i].GetWeight() <= toAdd.GetWeight()) {
                        sorter.Insert(i, toAdd);
                        add = true;
                        break;
                    }
                }
            }
            if (!add) {
                sorter.Add(toAdd);
            }
            Update?.Invoke();
        }

        public void Add(T toAdd, int weight) {
            WeightInstance<T> newWeightInstance = new WeightInstance<T>(toAdd, weight);
            Add(newWeightInstance);
        }

        public void Remove(WeightInstance<T> toRemove) {
            sorter.Remove(toRemove);
        }

        public void Remove(T toRemove) {
            for (int i = 0; i < sorter.Count; i++) {
                if (sorter[i].GetInstance().Equals(toRemove)) {
                    sorter.RemoveAt(i);
                }
            }
            Update?.Invoke();
        }

        public void AddListener(Action toAdd) {
            Update += toAdd;
        }

        public void RemoveListener(Action toRemove) {
            Update -= toRemove;
        }

        public T GetFirst() {
            if (sorter.Count == 0)
                return default(T);

            return sorter[0].GetInstance();
        }

        public List<T> GetInstances() {
            List<T> instances = new List<T>();
            for (int i = 0; i < sorter.Count; i++) {
                instances.Add(sorter[i].GetInstance());
            }
            return instances;
        }

        public class WeightInstance<U> {
            U instance;
            int weight;
            public WeightInstance(U instance, int weight) {
                this.instance = instance;
                this.weight = weight;
            }
            public U GetInstance() {
                return instance;
            }
            public int GetWeight() {
                return weight;
            }
        }

    }
}
