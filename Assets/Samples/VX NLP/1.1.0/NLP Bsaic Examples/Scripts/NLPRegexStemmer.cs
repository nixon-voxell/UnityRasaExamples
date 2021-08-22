using UnityEngine;
using Voxell;
using Voxell.NLP.Stem;
using Voxell.NLP.Tokenize;
using Voxell.Inspector;

public class NLPRegexStemmer : MonoBehaviour
{
  [StreamingAssetFilePath] public string tokenizerModel;
  [TextArea(1, 5)] public string sentence;
  public string[] tokens;
  public string[] stemmedTokens;

  private EnglishMaximumEntropyTokenizer _tokenizer;
  private RegexStemmer _regexStemmer;

  [Button]
  public void Stem()
  {
    _tokenizer = new EnglishMaximumEntropyTokenizer(FileUtil.GetStreamingAssetFilePath(tokenizerModel));
    _regexStemmer = new RegexStemmer();
    _regexStemmer.CreatePattern();

    // tokenize
    tokens = _tokenizer.Tokenize(sentence);
    stemmedTokens = new string[tokens.Length];
    // stem
    for (int t=0; t < tokens.Length; t++)
      stemmedTokens[t] = _regexStemmer.Stem(tokens[t]);
  }
}
