namespace NetBanking.Core.Application.Helpers
{
    public class CodeGenerator
    {
        public static string GenerateCode(string ProductCode, int ProductType)
        {

            Random rand = new Random();
            List<int> NumsGenerated = new List<int>();

            for (int i = 0; i <= 7; i++)
            {
                int randomNumber = rand.Next(1,9);
                NumsGenerated.Add(randomNumber);   
            }

            foreach (var nums in NumsGenerated)
            {
                ProductCode += nums;
            }
            return ProductCode;
        }
    }
}
