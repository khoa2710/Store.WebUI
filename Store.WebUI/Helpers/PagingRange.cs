namespace Store.WebUI.Helpers
{
    public class PagingRange
    {
        public static List<int> BuildPageRange(int current, int total, int max)
        {
            if (total <= max) 
            { 
                return Enumerable.Range(1, total).ToList(); 
            }

            int half = max / 2;
            int start = current - half;
            int end = current + half;

            if (start < 1) 
            { 
                end += 1 - start; start = 1; 
            }
            if (end > total) { 
                start -= end - total; end = total; 
            }

            return Enumerable.Range(start, max).ToList();
        }
    }
}
