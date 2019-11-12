using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NinjaCore.Extensions.Models
{
    public class BoundsValidationResult
    {
        public readonly bool IsValid;
        public readonly int IntendedSkip;
        public readonly int IntendedTake;
        public readonly ReadOnlyCollection<InvalidBounds> InvalidBounds;

        public BoundsValidationResult(int intendedSkip, int intendedTake, IEnumerable<InvalidBounds> invalidBounds)
        {
            IntendedSkip = intendedSkip;
            IntendedTake = intendedTake;
            InvalidBounds = new ReadOnlyCollection<InvalidBounds>(invalidBounds.ToList());
            IsValid = !InvalidBounds.Any();
        }
    }
}