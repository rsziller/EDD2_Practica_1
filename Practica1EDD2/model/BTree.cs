using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica1EDD2.model
{

    public class BTree<T> where T : IComparable<T>
    {
        private static int Z;

        public class Node
        {
            public int n;
            public Person[] key = new Person[(2 * Z - 1)];
            public Node[] child = new Node[(2 * Z)];
            public bool leaf = true;

            public int Find(Person k)
            {
                for (int i = 0; i < this.n; i++)
                {
                    if (this.key[i].Id == k.Id && this.key[i].Name == k.Name)
                        //if (this.key[i].Id == k.Id)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public int FindAll(Person k)
            {
                for (int i = 0; i < this.n; i++)
                {
                    if (this.key[i].Id == k.Id && this.key[i].Name == k.Name)
                    {
                        return i;
                    }
                }
                return -1;
            }
        }

        public Node root;

        public BTree(int z)
        {
            Z = z;
            root = new Node();
            root.n = 0;
            root.leaf = true;
        }


        public void PrintTreeGraph(string outputPath)
        {
            // Crear una descripción DOT del árbol B
            string dotContents = GenerateDotGraph();

            // Guardar la descripción DOT en un archivo temporal
            string dotFilePath = Path.Combine(Path.GetTempPath(), "btree.dot");
            File.WriteAllText(dotFilePath, dotContents);

            // Especificar la ubicación del ejecutable de Graphviz (asegúrate de tener Graphviz instalado)
            string graphvizPath = @"C:\Program Files\Graphviz\bin\dot.exe"; // Cambia la ruta según tu instalación

            // Generar una imagen PNG a partir del archivo DOT
            string outputImagePath = Path.Combine(outputPath, "btree.png");
            ProcessStartInfo psi = new ProcessStartInfo(graphvizPath)
            {
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                Arguments = $"-Tpng -o \"{outputImagePath}\" \"{dotFilePath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process() { StartInfo = psi })
            {
                process.Start();
                process.WaitForExit();
            }

            // Abrir la imagen generada
            //Process.Start(outputImagePath);
        }

        // Genera una descripción DOT del árbol B
        private string GenerateDotGraph()
        {
            int nodeCounter = 0; // Contador de nodos para asignar identificadores únicos

            // Crear una descripción DOT del árbol B
            string dotContents = "digraph BTree {\n";
            dotContents += "  node [shape=record];\n";

            // Función auxiliar para obtener un identificador único para un nodo
            string GetUniqueNodeId()
            {
                return $"node{nodeCounter++}";
            }

            // Función auxiliar para generar la descripción DOT de un nodo y sus hijos
            void GenerateNodeDot(Node node, string parentId)
            {
                string nodeId = GetUniqueNodeId();
                dotContents += $"  {nodeId} [label=\"";

                // Agregar claves del nodo al label
                for (int i = 0; i < node.n; i++)
                {
                    dotContents += $"<f{i}> {node.key[i].Id} {node.key[i].Name} {node.key[i].BirthDate} {node.key[i].Address}| ";
                }
                dotContents = dotContents.TrimEnd('|', ' ');

                dotContents += "\"];\n";

                if (!node.leaf)
                {
                    // Conectar el nodo padre con los hijos
                    for (int i = 0; i <= node.n; i++)
                    {
                        string childId = GetUniqueNodeId();
                        dotContents += $"  {nodeId}:f{i} -> {childId};\n";
                        GenerateNodeDot(node.child[i], childId); // Recursión para los hijos
                    }
                }

                // Conectar el nodo padre con el nodo actual
                if (parentId != null)
                {
                    dotContents += $"  {parentId} -> {nodeId};\n";
                }
            }

            // Comenzar la generación desde el nodo raíz
            GenerateNodeDot(root, null);

            dotContents += "}\n";
            return dotContents;
        }


        private Node Search(Node x, Person key)
        {
            int i = 0;
            if (x == null)
                return x;
            
            for (i = 0; i < x.n; i++)
            {
                if (key.CompareTo(x.key[i]) < 0)
                {
                    break;
                }
                if (key.CompareTo(x.key[i]) == 0)
                {
                    return x;
                }
            }
            if (x.leaf)
            {
                return null;
            }
            else
            {
                return Search(x.child[i], key);

            }

            
        }

        private void Split(Node x, int pos, Node y)
        {
            Node z = new Node();
            z.leaf = y.leaf;
            z.n = (Z - 1);
            for (int j = 0; j < (Z - 1); j++)
            {
                z.key[j] = y.key[j + Z];
            }
            if (!y.leaf)
            {
                for (int j = 0; j < Z; j++)
                {
                    z.child[j] = y.child[j + Z];
                }
            }
            y.n = (Z - 1);
            for (int j = x.n; j < pos ; j++)
            {
                x.child[j + 1] = x.child[j];
            }
            x.child[pos + 1] = z;

            for (int j = x.n; j < pos; j--)
            {
                x.key[j + 1] = x.key[j];
            }
            x.key[pos] = y.key[Z - 1];
            x.n++;
        }

        public void Insert(Person key)
        {
            Node r = root;
            if (r.n == (2 * Z - 1))
            {
                Node s = new Node();
                root = s;
                s.leaf = false;
                s.n = 0;
                s.child[0] = r;
                Split(s, 0, r);
                insertValue(s, key);
            }
            else
            {
                insertValue(r, key);
            }
        }

        /*private void insertValue(Node x, Person k)
        {
            if (x.leaf)
            {
                int i;
                for (i = x.n - 1; i >= 0 && k.CompareTo(x.key[i]) < 0; i--)
                {
                    x.key[i + 1] = x.key[i];
                }
                x.key[i + 1] = k;
                x.n = x.n + 1;
            }
            else
            {
                int i = 0;
                for (i = x.n - 1; i >= 0 && k.CompareTo(x.key[i]) < 0; i--)
                {
                }
                ;
                i++;
                Node tmp = x.child[i];
                if (tmp.n == 2 * Z - 1)
                {
                    Split(x, i, tmp);
                    if (k.CompareTo(x.key[i]) > 0)
                    {
                        i++;
                    }
                }
                insertValue(x.child[i], k);
            }
        }*/

        private void insertValue(Node x, Person k)
        {
            if (x.leaf)
            {
                int i = x.n;
                while (i >= 1 && k.CompareTo(x.key[i - 1]) < 0)
                {
                    x.key[i] = x.key[i - 1];
                    i--;
                }
                x.key[i] = k;
                x.n++;
            }
            else
            {
                int j = 0;
                while (j < x.n && k.CompareTo(x.key[j])>0)
                {
                    j++;
                }
                if (x.child[j].n == (2 * Z - 1))
                {
                    // (U - máximo de claves)
                    Split(x, j, x.child[j]);
                    if (k.CompareTo(x.key[j]) > 0)
                    {
                        j++;
                    }
                }
                insertValue(x.child[j], k);
            }
        }

        public void Show()
        {
            Show(root);
        }

        private void Show(Node x)
        {
            int i;
            for (i = 0; i < x.n; i++)
            {
                if (!x.leaf)
                    Show(x.child[i]);
                //Console.Write(" " + x.key[i].Name + " " + x.key[i].Id + " " + x.key[i].BirthDate + " " + x.key[i].Address + " ");
                Console.Write(" " + x.key[i].Name + " " + x.key[i].Id + " " + x.key[i].BirthDate + " " + x.key[i].Address + " ");
                Console.WriteLine("");
            }

            if (!x.leaf)
                Show(x.child[i]);
        }

 




        public bool Contain(Person k)
        {
            if (this.Search(root, k) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void Remove(Node x, Person key)
        {
            int pos = x.Find(key);
            if (pos != -1)
            {
                if (x.leaf)
                {
                    int i = 0;
                    for (i = 0; i < x.n && x.key[i] != key; i++)
                    {
                    }
                    ;
                    for (; i < x.n; i++)
                    {
                        if (i != 2 * Z - 2)
                        {
                            x.key[i] = x.key[i + 1];
                        }
                    }
                    x.n--;
                    return;
                }
                if (!x.leaf)
                {

                    Node pred = x.child[pos];
                    Person predKey;
                    if (pred.n >= Z)
                    {
                        for (; ; )
                        {
                            if (pred.leaf)
                            {
                                //Console.WriteLine(pred.n);
                                predKey = pred.key[pred.n - 1];
                                break;
                            }
                            else
                            {
                                pred = pred.child[pred.n];
                            }
                        }
                        Remove(pred, predKey);
                        x.key[pos] = predKey;
                        return;
                    }

                    Node nextNode = x.child[pos + 1];
                    if (nextNode.n >= Z)
                    {
                        Person nextKey = nextNode.key[0];
                        if (!nextNode.leaf)
                        {
                            nextNode = nextNode.child[0];
                            for (; ; )
                            {
                                if (nextNode.leaf)
                                {
                                    nextKey = nextNode.key[nextNode.n - 1];
                                    break;
                                }
                                else
                                {
                                    nextNode = nextNode.child[nextNode.n];
                                }
                            }
                        }
                        Remove(nextNode, nextKey);
                        x.key[pos] = nextKey;
                        return;
                    }

                    int temp = pred.n + 1;
                    pred.key[pred.n++] = x.key[pos];
                    for (int i = 0, j = pred.n; i < nextNode.n; i++)
                    {
                        pred.key[j++] = nextNode.key[i];
                        pred.n++;
                    }
                    for (int i = 0; i < nextNode.n + 1; i++)
                    {
                        pred.child[temp++] = nextNode.child[i];
                    }

                    x.child[pos] = pred;
                    for (int i = pos; i < x.n; i++)
                    {
                        if (i != 2 * Z - 2)
                        {
                            x.key[i] = x.key[i + 1];
                        }
                    }
                    for (int i = pos + 1; i < x.n + 1; i++)
                    {
                        if (i != 2 * Z - 1)
                        {
                            x.child[i] = x.child[i + 1];
                        }
                    }
                    x.n--;
                    if (x.n == 0)
                    {
                        if (x == root)
                        {
                            root = x.child[0];
                        }
                        x = x.child[0];
                    }
                    Remove(pred, key);
                    return;
                }
            }
            else
            {
                for (pos = 0; pos < x.n; pos++)
                {

                    if (key.CompareTo(x.key[pos]) < 0)
                    {
                        break;
                    }
                }
                Node tmp = x.child[pos];
                if (tmp.n >= Z)
                {
                    Remove(tmp, key);
                    return;
                }
                if (true)
                {
                    Node nb = null;
                    Person devider;

                    if (pos != x.n && x.child[pos + 1].n >= Z)
                    {
                        devider = x.key[pos];
                        nb = x.child[pos + 1];
                        x.key[pos] = nb.key[0];
                        tmp.key[tmp.n++] = devider;
                        tmp.child[tmp.n] = nb.child[0];
                        for (int i = 1; i < nb.n; i++)
                        {
                            nb.key[i - 1] = nb.key[i];
                        }
                        for (int i = 1; i <= nb.n; i++)
                        {
                            nb.child[i - 1] = nb.child[i];
                        }
                        nb.n--;
                        Remove(tmp, key);
                        return;
                    }
                    else if (pos != 0 && x.child[pos - 1].n >= Z)
                    {

                        devider = x.key[pos - 1];
                        nb = x.child[pos - 1];
                        x.key[pos - 1] = nb.key[nb.n - 1];
                        Node child = nb.child[nb.n];
                        nb.n--;

                        for (int i = tmp.n; i > 0; i--)
                        {
                            tmp.key[i] = tmp.key[i - 1];
                        }
                        tmp.key[0] = devider;
                        for (int i = tmp.n + 1; i > 0; i--)
                        {
                            tmp.child[i] = tmp.child[i - 1];
                        }
                        tmp.child[0] = child;
                        tmp.n++;
                        Remove(tmp, key);
                        return;
                    }
                    else
                    {
                        Node lt = null;
                        Node rt = null;
                        bool last = false;
                        if (pos != x.n)
                        {
                            devider = x.key[pos];
                            lt = x.child[pos];
                            rt = x.child[pos + 1];
                        }
                        else
                        {
                            devider = x.key[pos - 1];
                            rt = x.child[pos];
                            lt = x.child[pos - 1];
                            last = true;
                            pos--;
                        }
                        for (int i = pos; i < x.n - 1; i++)
                        {
                            x.key[i] = x.key[i + 1];
                        }
                        for (int i = pos + 1; i < x.n; i++)
                        {
                            x.child[i] = x.child[i + 1];
                        }
                        x.n--;
                        lt.key[lt.n++] = devider;

                        for (int i = 0, j = lt.n; i < rt.n + 1; i++, j++)
                        {
                            if (i < rt.n)
                            {
                                lt.key[j] = rt.key[i];
                            }
                            lt.child[j] = rt.child[i];
                        }
                        lt.n += rt.n;
                        if (x.n == 0)
                        {
                            if (x == root)
                            {
                                root = x.child[0];
                            }
                            x = x.child[0];
                        }
                        Remove(lt, key);
                        return;
                    }
                }
            }
        }

        public void Remove(Person key)
        {
            Node x = Search(root, key);
            if (x == null)
            {
                return;
            }
            Remove(root, key);
        }

        /*public void Remove(Person key)
        {
            Node x = Search(root, key);
            if (x == null)
            {
                return;
            }
            Remove(root, key);
        }*/

        public bool RemovePerson(string name, long dpi)
        {
            Person removedPerson = new Person();
            removedPerson.Name = name;
            removedPerson.Id = dpi;

            Node x = Search(root, removedPerson);
            if (x == null)
            {
                Console.WriteLine("No records found");
                return false;
            }

            //Console.WriteLine("vamos a borrar a: " + removedPerson.Id + removedPerson.Name);
            int personIndex = x.Find(removedPerson);
            Remove(root, x.key[personIndex]);
            return true;
        }
        //public bool UpdatePersonInfo(string name, long dpi, string newAddress = null, DateTime? newBirthDate = null)
        public bool UpdatePersonInfo(string name, long dpi, string newAddress = null, string newBirthDate = null)
        {
            // Crear una instancia de la persona con la nueva información
            Person updatedPerson = new Person();
            updatedPerson.Name = name;
            updatedPerson.Id = dpi;

            // Buscar la persona en el árbol
    
            Node nodeContainingPerson = Search(root, updatedPerson);

            if (nodeContainingPerson == null)
            {
                // La persona no se encontró en el árbol
                Console.WriteLine("No records found");
                return false;
            }

            int personIndex = nodeContainingPerson.Find(updatedPerson);
            
            if (personIndex != -1)
            {
                // Actualizar la dirección y la fecha de nacimiento
                //nodeContainingPerson.key[personIndex].Address = newAddress;
                //nodeContainingPerson.key[personIndex].BirthDate = newBirthDate.Value;

                // Aquí puedes también actualizar otros campos si es necesario
                if (newAddress != null)
                {
                    nodeContainingPerson.key[personIndex].Address = newAddress;
                }

                //if (newBirthDate.HasValue)
                if (newBirthDate != null)
                {
                    //nodeContainingPerson.key[personIndex].BirthDate = newBirthDate.Value;
                    nodeContainingPerson.key[personIndex].BirthDate = newBirthDate;
                }

                return true; // Actualización exitosa
            }

            return false; // La persona no se encontró en el nodo, no se pudo actualizar
        }



        public List<Person> SearchByName(string name)
        {
            List<Person> matches = new List<Person>();
            SearchByName(root, name, matches);
            return matches;
        }

        private void SearchByName(Node x, string name, List<Person> matches)
        {
            if (x == null)
                return;

            for (int i = 0; i < x.n; i++)
            {
                // Verifica si el nombre de la persona coincide
                

                // Si el nodo no es una hoja, busca en los hijos recursivamente
                if (!x.leaf)
                {
                    SearchByName(x.child[i], name, matches);
                }

                if (x.key[i].Name.Contains(name))
                {
                    matches.Add(x.key[i]);
                }
            }

            // Busca en el último hijo si no es una hoja
            if (!x.leaf)
            {
                SearchByName(x.child[x.n], name, matches);
            }
        }




    }

}
