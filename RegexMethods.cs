using System.Text.RegularExpressions; // regex


namespace RpgGame
{
  public class RegexMethods {
    /// <summary>
    /// Checks if in Character Name is an InValid Sign
    /// </summary>
    /// <param name="input"></param>
    /// <returns>true if wrong sign / false - if all correct</returns>
    public static bool IsInValidSign(string input) {
      Regex regex = new Regex("[\\\\/:\\*\\?\"<>\\|]", RegexOptions.IgnoreCase);
      return regex.IsMatch(input);
    }
  }
}
