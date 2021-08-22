using UnityEngine;
using Voxell;
using Voxell.NLP.Tokenize;
using Voxell.Inspector;

public class NLPTokenizer : MonoBehaviour
{
  [StreamingAssetFilePath] public string tokenizerModel;
  [TextArea(1, 5)] public string sentence;
  public string[] tokens;

  private EnglishMaximumEntropyTokenizer _tokenizer;

  [Button]
  public void Tokenize()
  {
    _tokenizer = new EnglishMaximumEntropyTokenizer(FileUtil.GetStreamingAssetFilePath(tokenizerModel));
    tokens = _tokenizer.Tokenize(sentence);
  }
}
