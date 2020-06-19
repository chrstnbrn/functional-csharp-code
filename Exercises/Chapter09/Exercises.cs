using LaYumba.Functional.Data.BinaryTree;
using LaYumba.Functional.Data.LinkedList;
using System;
using static LaYumba.Functional.Data.LinkedList.LinkedList;
using static LaYumba.Functional.Data.BinaryTree.Tree;
using System.Reflection.Emit;
using NUnit.Framework;

namespace Exercises.Chapter9
{
    static class Exercises
    {
        // LISTS

        // Implement functions to work with the singly linked List defined in this chapter:
        // Tip: start by writing the function signature in arrow-notation

        // InsertAt inserts an item at the given index
        // InsertAt: List<T> -> int -> T -> List<T>
        static List<T> InsertAt<T>(this List<T> ts, int index, T item)
            => index == 0
                  ? List(item, ts)
                  : List(ts.Head, ts.Tail.InsertAt(index - 1, item));

        // RemoveAt removes the item at the given index
        // RemoveAt: List<T> -> int -> List<T>
        static List<T> RemoveAt<T>(this List<T> ts, int index)
            => index == 0
                ? ts.Tail
                : List(ts.Head, ts.Tail.RemoveAt(index - 1));

        // TakeWhile takes a predicate, and traverses the list yielding all items until it find one that fails the predicate
        // TakeWhile: List<T> -> Func<T, bool> -> List<T>
        static List<T> TakeWhile<T>(this List<T> ts, Func<T, bool> predicate)
            => ts.Match(
                () => ts,
                (head, tail) => predicate(head) ? List(head, tail.TakeWhile(predicate)) : List<T>());

        // DropWhile works similarly, but excludes all items at the front of the list
        // DropWhile: List<T> -> Func<T, bool> -> List<T>
        static List<T> DropWhile<T>(this List<T> ts, Func<T, bool> predicate)
            => ts.Match(
                () => ts,
                (head, tail) => predicate(head) ? tail.DropWhile(predicate) : ts);

        // complexity:
        // InsertAt: O(index)
        // RemoveAt: O(index)
        // TakeWhile: O(m) where m = number of elements for which the predicate returns true
        // DropWhile: O(m) where m = number of elements for which the predicate returns false

        // number of new objects required: 
        // InsertAt: index
        // RemoveAt: index
        // TakeWhile: m
        // DropWhile: 0 

        // TakeWhile and DropWhile are useful when working with a list that is sorted 
        // and you’d like to get all items greater/smaller than some value; write implementations 
        // that take an IEnumerable rather than a List

        static System.Collections.Generic.IEnumerable<T> TakeWhile<T>(this System.Collections.Generic.IEnumerable<T> ts, Func<T, bool> predicate)
        {
            foreach(var t in ts)
            {
                if (predicate(t))
                    yield return t;
                else
                    yield break;
            }
        }

        static System.Collections.Generic.IEnumerable<T> DropWhile<T>(this System.Collections.Generic.IEnumerable<T> ts, Func<T, bool> predicate)
        {
            var predicateWasTrue = false;
            foreach(var t in ts)
            {
                if (predicateWasTrue || predicate(t)) {
                    yield return t;
                    predicateWasTrue = true;
                }
            }
        }


        // TREES

        // Is it possible to define `Bind` for the binary tree implementation shown in this
        // chapter? If so, implement `Bind`, else explain why it’s not possible (hint: start by writing
        // the signature; then sketch binary tree and how you could apply a tree-returning funciton to
        // each value in the tree).

        // Bind: Tree<T> -> Func<T, Tree<R>> -> Tree<R>
        static Tree<R> Bind<T, R>(this Tree<T> tree, Func<T, Tree<R>> f)
        {
            return tree.Match(
                Leaf: f,
                Branch: (left, right) => Branch(left.Bind(f), right.Bind(f)));
        }

        // Implement a LabelTree type, where each node has a label of type string and a list of subtrees; 
        // this could be used to model a typical navigation tree or a cateory tree in a website

        public class LabelTree
        {
            public string Label { get; }
            public List<LabelTree> Subtrees { get; }

            public LabelTree(string label, List<LabelTree> subtrees = null)
            {
                Label = label;
                Subtrees = subtrees ?? List<LabelTree>();
            }

            public override string ToString() => $"{Label}: {Subtrees}";
            public override bool Equals(object other) => this.ToString() == other.ToString();
        }


        // Imagine you need to add localization to your navigation tree: you're given a `LabelTree` where
        // the value of each label is a key, and a dictionary that maps keys
        // to translations in one of the languages that your site must support
        // (hint: define `Map` for `LabelTree` and use it to obtain the localized navigation/category tree)

        static LabelTree Map(this LabelTree tree, Func<string, string> f)
            => new LabelTree(f(tree.Label), tree.Subtrees.Map(t => t.Map(f)));

        [Test]
        public static void TestLabelTree()
        {
            var labelTree = new LabelTree(
                "Animals",
                List(
                    new LabelTree(
                        "Birds",
                        List(new LabelTree("Crow"), new LabelTree("Eagle"))
                    ),
                    new LabelTree(
                        "Mammals",
                        List(new LabelTree("Dog"), new LabelTree("Cat"))
                    )
                )
            );
            var germanDictionary = new System.Collections.Generic.Dictionary<string, string>
            {
                ["Animals"] = "Tiere",
                ["Birds"] = "Vögel",
                ["Mammals"] = "Säugetiere",
                ["Crow"] = "Krähe",
                ["Eagle"] = "Adler",
                ["Dog"] = "Hund",
                ["Cat"] = "Katze"
            };

            var actual = labelTree.Map(l => germanDictionary[l]);

            var expected = new LabelTree(
                "Tiere",
                List(
                    new LabelTree(
                        "Vögel",
                        List(new LabelTree("Krähe"), new LabelTree("Adler"))
                    ),
                    new LabelTree(
                        "Säugetiere",
                        List(new LabelTree("Hund"), new LabelTree("Katze"))
                    )
                )
            );
            Assert.AreEqual(expected, actual);
        }
    }
}