using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ActionP
{
    public string Name { get; set; }
}

public class RelationP
{
    public string Name { get; set; }
    public HashSet<string> PermittedActions { get; set; } = new HashSet<string>();
}

public class AssignmentP
{
    public string Subject { get; set; }
    public string RelationName { get; set; }
    public string Object { get; set; }
}

public class RelationParser
{
    private const string ActionPattern = @"DEFINE ACTION (\w+)";
    private const string RelationPattern = @"DEFINE RELATION (\w+) \{([\s\S]*?)\}";
    private const string PermitsPattern = @"PERMITS: (\w+),?";
    private const string AssignmentPattern = @"ASSIGN (\w+) AS (\w+) TO (\w+)";

    public Dictionary<string, ActionP> ParseActions(IEnumerable<string> inputs)
    {
        var actions = new Dictionary<string, ActionP>();
        foreach (var input in inputs)
        {
            var match = Regex.Match(input, ActionPattern);
            if (match.Success)
            {
                var action = new ActionP { Name = match.Groups[1].Value };
                actions[action.Name] = action;
            }
        }
        return actions;
    }

    public Dictionary<string, RelationP> ParseRelations(IEnumerable<string> inputs, Dictionary<string, ActionP> actions)
    {
        var relations = new Dictionary<string, RelationP>();
        foreach (var input in inputs)
        {
            var relationMatch = Regex.Match(input, RelationPattern);
            if (relationMatch.Success)
            {
                var relation = new RelationP { Name = relationMatch.Groups[1].Value };

                var permitsBlock = relationMatch.Groups[2].Value;
                var permitsMatches = Regex.Matches(permitsBlock, PermitsPattern);
                foreach (Match permitsMatch in permitsMatches)
                {
                    var actionName = permitsMatch.Groups[1].Value;
                    if (!actions.ContainsKey(actionName))
                    {
                        throw new ArgumentException($"Action '{actionName}' is not defined.");
                    }
                    relation.PermittedActions.Add(actionName);
                }

                relations[relation.Name] = relation;
            }
        }
        return relations;
    }

    public List<AssignmentP> ParseAssignments(IEnumerable<string> inputs, Dictionary<string, RelationP> relations)
    {
        var assignments = new List<AssignmentP>();
        foreach (var input in inputs)
        {
            var match = Regex.Match(input, AssignmentPattern);
            if (match.Success)
            {
                var relationName = match.Groups[2].Value;
                if (!relations.ContainsKey(relationName))
                {
                    throw new ArgumentException($"Relation '{relationName}' is not defined.");
                }

                var assignment = new AssignmentP
                {
                    Subject = match.Groups[1].Value,
                    RelationName = relationName,
                    Object = match.Groups[3].Value
                };
                assignments.Add(assignment);
            }
        }
        return assignments;
    }
}

// // Example usage of the parser
// public class Program
// {
//     public static void Main()
//     {
//         var parser = new RelationParser();
//         var inputs = new List<string>
//         {
//           
//             "DEFINE ACTION read",
//             "DEFINE ACTION write",
//             "ASSIGN User123 AS Editor TO Document456",
//             "DEFINE ACTION delete",
//             "DEFINE ACTION approve",
//             "DEFINE RELATION Editor { PERMITS: read, PERMITS: write, PERMITS: approve }",
//             // More definitions can be added here
//         };
//
//         var actions = parser.ParseActions(inputs);
//         var relations = parser.ParseRelations(inputs, actions);
//         var assignments = parser.ParseAssignments(inputs, relations);
//
//         
//     }
// }
