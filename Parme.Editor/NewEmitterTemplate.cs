using System.ComponentModel.DataAnnotations;

namespace Parme.Editor
{
    public enum NewEmitterTemplate
    {
        [Display(Name = "<None>")]
        None = 0,
        
        [Display(Name = "Fire Effect")]
        Fire = 1,
    }
}