namespace SerializeDeserializeDLList
{
    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            using (BinaryWriter w = new BinaryWriter(s))
            {
                w.Write(Count);

                ListNode current = Head;
                List<ListNode> arr = new List<ListNode>();

                while (current != null)
                {
                    arr.Add(current);
                    w.Write(current.Data);
                    current = current.Next;
                }

                for (int i = 0; i < arr.Count; i++)
                {
                    int randIndex = -1;
                    if (arr[i].Rand != null)
                    {
                        randIndex = arr.IndexOf(arr[i].Rand);
                    }

                    w.Write(randIndex);
                }
            }
        }

        public void Deserialize(FileStream s)
        {
            List<ListNode> arr = new List<ListNode>();
            Count = 0;
            Head = null;
            Tail = null;

            using (BinaryReader r = new BinaryReader(s))
            {
                Count = r.ReadInt32();

                for (int i = 0; i < Count; i++)
                {
                    ListNode node = new ListNode();
                    arr.Add(node);

                    node.Data = r.ReadString();

                    if (Head == null)
                    {
                        Head = node;
                    }
                    else
                    {
                        Tail.Next = node;
                        node.Prev = Tail;
                    }

                    Tail = node;

                    if (Tail.Next != null)
                    {
                        throw new Exception("List structure is invalid");
                    }
                }

                if (Count != arr.Count)
                {
                    throw new Exception("List structure is invalid");
                }

                for (int i = 0; i < Count; i++)
                {
                    int randIndex = r.ReadInt32();
                    if (randIndex >= 0)
                    {
                        arr[i].Rand = arr[randIndex];
                    }
                }
            }
        }
    }
}
/*
    Данный алгоритм представляет реализацию методов Serialize и Deserialize для класса ListRand, который представляет двунаправленный связный список с указателем на произвольный узел (Rand) и подсчетом количества элементов (Count).
    
    Сложность алгоритма зависит от размера списка и количества ссылок на произвольные узлы. Для метода Serialize алгоритм имеет сложность O(n), где n - количество элементов в списке, так как мы проходимся по каждому элементу списка один раз и записываем данные в поток. Затем мы проходимся по списку еще раз, чтобы записать индексы ссылок на произвольные узлы, что также требует O(n) операций.

    Для метода Deserialize алгоритм также имеет сложность O(n), где n - количество элементов в списке, так как мы проходимся по каждому элементу в потоке и создаем новый узел, который добавляем в список. Затем мы проходимся по списку еще раз, чтобы установить ссылки на произвольные узлы, что также требует O(n) операций.

    Таким образом, общая сложность алгоритма равна O(n), где n - количество элементов в списке.
*/