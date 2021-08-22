using UnityEngine;
using Voxell;
using Voxell.NLP.PosTagger;
using Voxell.NLP.Tokenize;
using Voxell.Inspector;

public class NLPPOSTagger : MonoBehaviour
{
  [StreamingAssetFilePath] public string tokenizerModel;
  [StreamingAssetFilePath] public string posTaggerModel;
  [StreamingAssetFilePath] public string tagDict;
  [TextArea(1, 5)] public string sentence;
  public string[] tokens;
  public string[] posTags;

  private EnglishMaximumEntropyTokenizer _tokenizer;
  private EnglishMaximumEntropyPosTagger _posTagger;

  [Button]
  public void Tag()
  {
    // link to POS tags meanings: https://www.ling.upenn.edu/courses/Fall_2003/ling001/penn_treebank_pos.html
    _tokenizer = new EnglishMaximumEntropyTokenizer(FileUtil.GetStreamingAssetFilePath(tokenizerModel));
    _posTagger = new EnglishMaximumEntropyPosTagger(
      FileUtil.GetStreamingAssetFilePath(posTaggerModel),
      FileUtil.GetStreamingAssetFilePath(tagDict));

    tokens = _tokenizer.Tokenize(sentence);
    posTags = _posTagger.Tag(tokens);
  }
}
