using System;
using System.Collections.Generic;
using System.Linq;

namespace YewLib.Util
{
    public class LCS<T>
    {
        public Func<T, T, bool> EqualityTest = (a, b) => Equals(a, b);
        
        public bool Print { get; set; }

        public int Longest(IList<T> src, IList<T> dst)
        {
            var (n, m) = (src.Count, dst.Count);
            return LongestCommonSubsequence(src, dst)[n, m].lcs;
        }

        public IEnumerable<LcsOp> Path(IList<T> src, IList<T> dst)
        {
            var (i, j) = (src.Count, dst.Count);
            var dp = LongestCommonSubsequence(src, dst);
            var stack = new Stack<LcsOp>();
            while (i > 0 || j > 0)
            {
                var op = dp[i, j].op;
                stack.Push(op);
                switch (op)
                {
                    case LcsOp.Keep:
                        i--;
                        j--;
                        break;
                    case LcsOp.Delete:
                        i--;
                        break;
                    case LcsOp.Insert:
                        j--;
                        break;
                }
            }

            while (stack.Any()) yield return stack.Pop();
        }

        // private (int cost, char op)[,] dp = new (int cost, char op)[100, 100];
        public (int lcs, LcsOp op)[,] LongestCommonSubsequence(IList<T> src, IList<T> dst)
        {
            int i, j;
            var (n, m) = (src.Count, dst.Count);
            var dp = new (int lcs, LcsOp op)[n + 1, m + 1];
            dp[0, 0] = (0, LcsOp.Keep);
            for (i = 1; i <= n; i++)
                dp[i, 0] = (0, LcsOp.Delete);
            for (j = 1; j <= m; j++)
                dp[0, j] = (0, LcsOp.Insert);
            for (i = 1; i <= n; i++)
            {
                for (j = 1; j <= m; j++)
                {
                    if (EqualityTest(src[i - 1], dst[j - 1]))
                        dp[i, j] = (1 + dp[i - 1, j - 1].lcs, LcsOp.Keep);
                    else if (dp[i - 1, j].lcs > dp[i, j - 1].lcs)
                    {
                        dp[i, j] = (dp[i - 1, j].lcs, LcsOp.Delete);
                    }
                    else
                    {
                        dp[i, j] = (dp[i, j - 1].lcs, LcsOp.Insert);
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

    public enum LcsOp
    {
        Insert,
        Keep,
        Delete
    }
}