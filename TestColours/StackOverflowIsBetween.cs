namespace TestColours
{
    /// <summary>
    /// Is there a "between" function in C#?
    /// https://stackoverflow.com/questions/5023213/is-there-a-between-function-in-c
    /// Question: "Google doesn't understand that "between" is the name of the function I'm looking for and returns nothing relevant.
    ///            Ex: I want to check if 5 is between 0 and 10 in only one operation"
    /// 
    /// Ans: "It isn't clear what you mean by "one operation", but no, there's no operator / framework method that I know of to determine if an item is within a range.
    ///       You could of course write an extension-method yourself.For example, here's one that assumes that the interval is closed on both end-points."
    /// </summary>
    public static class StackOverflowIsBetween
    {
        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }
    }
}
