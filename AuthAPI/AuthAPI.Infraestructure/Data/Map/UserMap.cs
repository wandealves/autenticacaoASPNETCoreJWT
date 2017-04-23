using AuthAPI.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthAPI.Infraestructure.Data.Map
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Name)
               .HasMaxLength(250)
               .IsRequired();

            entityBuilder.Property(x => x.Login)
               .HasMaxLength(45)
               .IsRequired();

            entityBuilder.Property(x => x.Email)
              .HasMaxLength(45);

            entityBuilder.Property(x => x.Password)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
