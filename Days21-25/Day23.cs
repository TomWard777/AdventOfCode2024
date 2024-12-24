namespace AdventOfCode2024;

public class Day23
{
    public void Run()
    {
        //var input = FileParser.ReadInputFromFile("Day23.txt");
        var input = FileParser.ReadInputFromFile("Test23.txt");

        var graph = GetGraphFromInput(input);
        var triangles = GetTriangles(graph);

        var result = triangles.Count(t => t[0].StartsWith('t') || t[1].StartsWith('t') || t[2].StartsWith('t'));
        Console.WriteLine("Part 1 result = " + result);

        var list = graph.Vertices.Select(v => (v, graph.Degree(v)))
        .OrderByDescending(pair => pair.Item2)
        .ToArray();

        foreach(var x in list)
        {
            Console.Write($"({x.v}, {x.Item2}) ,");
        }
    }

    public List<string[]> GetTriangles(Graph graph)
    {
        var triangleStrings = new List<string>();

        // We are overcounting by 3.
        foreach(var edge in graph.Edges)
        {
            var edges1 = graph.GetIncidentEdges(edge.Vertex1).Where(e => !e.Equals(edge));
            var edges2 = graph.GetIncidentEdges(edge.Vertex2).Where(e => !e.Equals(edge));

            foreach(var e1 in edges1)
            {
                var v = e1.Vertex1 == edge.Vertex1 ? e1.Vertex2 : e1.Vertex1;

                // There can be at most one edge forming a triangle
                var thirdTriangeEdge = edges2
                .Where(e => e.Vertex1 == v || e.Vertex2 == v)
                .FirstOrDefault();

                if(thirdTriangeEdge != null)
                {
                    var list = new List<string> {edge.Vertex1, edge.Vertex2, v};

                    var str = string.Join(',', list.OrderBy(s => s));
                    triangleStrings.Add(str);
                }
            }
        }

        return triangleStrings
        .Distinct()
        .Select(str => str.Split(",")).ToList();
    }

    public Graph GetGraphFromInput(IEnumerable<string> input)
    {
        var graph = new Graph();
        
        graph.Edges = input.Select(str => str.Split('-'))
        .Select(arr => new Edge(arr[0], arr[1]))
        .ToList();

        var vertices = graph.Edges.Select(e => e.Vertex1).ToList();
        var vertices2 = graph.Edges.Select(e => e.Vertex2).ToList();

        vertices.AddRange(vertices2);
        graph.Vertices = vertices.Distinct().ToList();

        return graph;
    }

    public void PrintEdge(Edge e) => Console.WriteLine(e.Vertex1 + "," + e.Vertex2);
}

public class Graph
{
    public Graph()
    {
        Vertices = new List<string>();
        Edges = new List<Edge>();
    }

    public List<string> Vertices { get; set; }
    public List<Edge> Edges { get; set; }

    public List<Edge> GetIncidentEdges(string vertex)
    {
        return Edges.Where(e => e.Vertex1 == vertex || e.Vertex2 == vertex).ToList();
    }

    public int Degree(string vertex)
    {
        return GetIncidentEdges(vertex).Count;
    }

    public bool Adjacent(string v, string w)
    {
        return Edges.Any(e => (e.Vertex1 == v && e.Vertex2 == w) || (e.Vertex1 == w && e.Vertex2 == v));
    }
}

public class Edge : IEquatable<Edge>
{
    public Edge(string vertex1, string vertex2)
    {
        if (vertex1 == vertex2)
        {
            throw new Exception("Edge cannot be a loop");
        }

        Vertex1 = vertex1;
        Vertex2 = vertex2;
    }

    public string Vertex1 { get; set; }
    public string Vertex2 { get; set; }

    public bool Equals(Edge? otherEdge)
    {
        if (otherEdge == null)
        {
            return false;
        }

        return ((otherEdge.Vertex1 == Vertex1 && otherEdge.Vertex2 == Vertex2)
        || (otherEdge.Vertex2 == Vertex1 && otherEdge.Vertex1 == Vertex2));
    }
}