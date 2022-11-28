using System.Text.RegularExpressions;
using Xunit;
namespace NameInverter
{
    public class NameInverter
    {
        private void AssertMyName(string input, string expected)
        {
            Assert.Equal(expected, Invert(input));
        }

        [Fact]
        public void GivenNullThenIGetEmptyString()
        {
            AssertMyName(null!, "");
        }

        [Fact]
        public void GivenEmptyNameThenIgetEmptyName()
        {
            AssertMyName("", "");
        }

        [Fact]
        public void GivenSimpleNameThenIgetSimpleName()
        {
            AssertMyName("Rama", "Rama");

        }

        [Fact]
        public void GivenRamaChitturThenIGetChitturiRama()
        {
            AssertMyName("Rama Chitturi", "Chitturi, Rama");
        }

        [Fact]
        public void GivenMoreSpacesThenIGetLastFirst()
        {
            AssertMyName("   Rama   Chitturi  ", "Chitturi, Rama");
        }

        [Fact]
        public void IgnoreHonors()
        {
            AssertMyName("Mr. Rama Chitturi", "Chitturi, Rama");
            AssertMyName("Mrs. Ramya Chitturi", "Chitturi, Ramya");
            AssertMyName("Dr. Ramya Chitturi", "Chitturi, Ramya");
        }

        [Fact]
        public void PostNominalsAttheEnd()
        {
            AssertMyName("Rama Chitturi MBBS", "Chitturi, Rama MBBS");
            AssertMyName("Rama Chitturi MBBS Phd.", "Chitturi, Rama MBBS Phd.");
        }

        [Fact]
        public void BigName()
        {
            AssertMyName("    Mr. Rama     Chitturi    MSc. Phd.    ", "Chitturi, Rama MSc. Phd.");
        }

        public string Invert(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            else
            {
                return FormatName(SplitName(input));
            }
        }

        private static string FormatName(List<string> names)
        {
            if (names.Count == 1)
            {
                return names[0];
            }
            else
            {
                return FormatMultiNames(names);
            }
        }

        private static string FormatMultiNames(List<string> names)
        {
            string postNominal = GetPostNominal(RemoveHonors(names));
            return FormatName(names, postNominal);
        }

        private static string FormatName(List<string> names, string postNominal)
        {
            string lastName = names[1];
            string firstName = names[0];    
            return $"{lastName}, {firstName} {postNominal}".Trim();
        }

        private static List<string> SplitName(string? input)
        {
            Regex regex = new Regex("\\s+");
            var names = regex.Split(input!.Trim()).ToList();
            return names;
        }

        private static List<string> RemoveHonors(List<string> names)
        {
            List<string> honors = new List<string> { "Mr.", "Mrs.", "Dr." };

            if (names.Count > 2 && honors.Contains(names[0]))
            {
                names.RemoveAt(0);
            }
            return names;
        }

        private static string GetPostNominal(List<string> names)
        {
            string postNominal = "";
            if (names.Count > 2)
            {
                var postNominals = names.GetRange(2, names.Count() - 2);
                foreach (var name in postNominals)
                {
                    postNominal += name + " ";
                }
            }
            return postNominal;
        }
    }
}