﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleRoslynHelper;

/// <summary>
/// The default Roslyn Extensions.
/// </summary>
public static class RoslynExtensions
{
    /// <summary>
    /// All the Children in this type.
    /// </summary>
    /// <typeparam name="T">The node type</typeparam>
    /// <param name="node"></param>
    /// <param name="removedNodes">the nodes need to removed.</param>
    /// <returns></returns>
    public static IEnumerable<T> GetChildren<T>(this SyntaxNode node, params SyntaxNode[] removedNodes) where T : SyntaxNode
    {
        if (removedNodes.Contains(node)) return [];
        if (node is T result) return [result];
        return node.ChildNodes().SelectMany(n => n.GetChildren<T>(removedNodes));
    }

    /// <summary>
    /// Get the first parent with the specific <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The node type</typeparam>
    /// <param name="node"></param>
    /// <returns></returns>
    public static T? GetParent<T>(this SyntaxNode? node) where T : SyntaxNode
    {
        if (node == null) return null;
        if (node is T result) return result;
        return GetParent<T>(node.Parent);
    }

    /// <summary>
    /// Get the full symbol name.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string GetFullMetadataName(this ISymbol s)
    {
        if (s == null || s is INamespaceSymbol)
        {
            return string.Empty;
        }

        while (s != null && s is not ITypeSymbol)
        {
            s = s.ContainingSymbol;
        }

        if (s == null)
        {
            return string.Empty;
        }

        var sb = new StringBuilder(s.GetTypeSymbolName());

        s = s.ContainingSymbol;
        while (!IsRootNamespace(s))
        {
            try
            {
                sb.Insert(0, s.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat) + '.');
            }
            catch
            {
                break;
            }

            s = s.ContainingSymbol;
        }

        return sb.ToString();

        static bool IsRootNamespace(ISymbol symbol)
        {
            return symbol is INamespaceSymbol s && s.IsGlobalNamespace;
        }
    }

    private static string GetTypeSymbolName(this ISymbol symbol)
    {
        if (symbol is IArrayTypeSymbol arrayTypeSymbol) //Array
        {
            return arrayTypeSymbol.ElementType.GetFullMetadataName() + "[]";
        }

        var str = symbol.MetadataName;
        if (symbol is INamedTypeSymbol symbolType)//Generic
        {
            var strs = str.Split('`');
            if (strs.Length < 2) return str;
            str = strs[0];

            str += "<" + string.Join(", ", symbolType.TypeArguments.Select(p => p.GetFullMetadataName())) + ">";
        }
        return str;
    }

    /// <summary>
    /// Print a node to string.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static string NodeToString(this SyntaxNode node)
    {
        using var stringWriter = new StringWriter();
        node.NormalizeWhitespace().WriteTo(stringWriter);
        return stringWriter.ToString();
    }
}
