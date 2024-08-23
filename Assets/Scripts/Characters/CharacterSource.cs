using DataSources;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(menuName = "Data/Sources/Character", fileName = "Source_CharacterData")]
    public class CharacterSource : DataSource<Character> { }
}