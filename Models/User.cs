using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lebonanimal.Models;

public class User
{
    [Key] public int Id { get; set; }
    
    [Required]
    [MaxLength(50,ErrorMessage = "La taille max est 50")]
    [DisplayName("Prénom")] public string Firstname { get; set; }
    
    [Required]
    [MaxLength(50,ErrorMessage = "La taille max est 50")]
    [DisplayName("Nom")] public string Lastname { get; set; }

    [Required]
    [EmailAddress]
    [DisplayName("Email")]
    [MaxLength(320,ErrorMessage = "La taille max est 320")]
    public string Email { get; set; }

    [Required]
    [MinLength(10, ErrorMessage = "Le mot de passe doit faire au moins 10 caractères")]
    [Compare("ConfirmPassword",
        ErrorMessage = "Le mot de passe n'est pas le même que le mot de passe de confirmation")]
    [DisplayName("Mot de passe")]
    public string Password { get; set; }

    [Required]
    [MinLength(10, ErrorMessage = "Le mot de passe doit faire au moins 10 caractères")]
    [NotMapped] // Does not effect with your database
    [DisplayName("Confirmer le mot de passe")]
    public string ConfirmPassword { get; set; }

    [DefaultValue(false)]
    public bool Banned { get; set; }
    [DefaultValue(false)]
    public bool Admin { get; set; }
}