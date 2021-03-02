using System;
using System.Collections.Generic;
using System.Linq;

namespace YewLib.Util
{
    public class LCS
    {
        public enum Op
        {
            Insert,
            Keep,
            Delete
        }
        
        public bool Print { get; set; }

        public int Longest<T>(IList<T> src, IList<T> dst)
        {
            var (n, m) = (src.Count, dst.Count);
            return LongestCommonSubsequence(src, dst)[n, m].lcs;
        }

        public IEnumerable<Op> Path<T>(IList<T> src, IList<T> dst)
        {
            var (i, j) = (src.Count, dst.Count);
            var dp = LongestCommonSubsequence<T>(src, dst);
            var stack = new Stack<Op>();
            while (i > 0 || j > 0)
            {
                var op = dp[i, j].op;
                stack.Push(op);
                switch (op)
                {
                    case Op.Keep:
                        i--;
                        j--;
                        break;
                    case Op.Delete:
                        i--;
                        break;
                    case Op.Insert:
                        j--;
                        break;
                }
            }

            while (stack.Any()) yield return stack.Pop();
        }

        // private (int cost, char op)[,] dp = new (int cost, char op)[100, 100];
        public (int lcs, Op op)[,] LongestCommonSubsequence<T>(IList<T> src, IList<T> dst)
        {
            int i, j;
            var (n, m) = (src.Count, dst.Count);
            var dp = new (int lcs, Op op)[n + 1, m + 1];
            dp[0, 0] = (0, Op.Keep);
            for (i = 1; i <= n; i++)
                dp[i, 0] = (0, Op.Delete);
            for (j = 1; j <= m; j++)
                dp[0, j] = (0, Op.Insert);
            for (i = 1; i <= n; i++)
            {
                for (j = 1; j <= m; j++)
                {
                    if (Equals(src[i - 1], dst[j - 1]))
                        dp[i, j] = (1 + dp[i - 1, j - 1].lcs, Op.Keep);
                    else if (dp[i - 1, j].lcs > dp[i, j - 1].lcs)
                    {
                        dp[i, j] = (dp[i - 1, j].lcs, Op.Delete);
                    }
                    else
                    {
                        dp[i, j] = (dp[i, j - 1].lcs, Op.Insert);
                    }
                }
            }

            if (Print)
            {
                for (i = 0; i <= n; i++)
                {
                    for (j = 0; j <= m; j++)
                    {
                        Console.Write(dp[i, j] + "  ");
                    }

                    Console.WriteLine();
                }
            }

            return dp;
        }
    }
}