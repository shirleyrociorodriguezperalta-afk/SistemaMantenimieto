using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaMantenimieto.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tecnico",
                columns: table => new
                {
                    IdTecnico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Especialidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecnico", x => x.IdTecnico);
                });

            migrationBuilder.CreateTable(
                name: "Tipo_Equipo",
                columns: table => new
                {
                    IdTipoEquipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo_Equipo", x => x.IdTipoEquipo);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Equipo",
                columns: table => new
                {
                    IdEquipo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoActivo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdTipoEquipo = table.Column<int>(type: "int", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EstadoOperativo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipo", x => x.IdEquipo);
                    table.ForeignKey(
                        name: "FK_Equipo_Tipo_Equipo_IdTipoEquipo",
                        column: x => x.IdTipoEquipo,
                        principalTable: "Tipo_Equipo",
                        principalColumn: "IdTipoEquipo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanMantenimiento",
                columns: table => new
                {
                    IdPlan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEquipo = table.Column<int>(type: "int", nullable: false),
                    DescripcionTarea = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FrecuenciaDias = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanMantenimiento", x => x.IdPlan);
                    table.ForeignKey(
                        name: "FK_PlanMantenimiento_Equipo_IdEquipo",
                        column: x => x.IdEquipo,
                        principalTable: "Equipo",
                        principalColumn: "IdEquipo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mantenimiento",
                columns: table => new
                {
                    IdMantenimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPlan = table.Column<int>(type: "int", nullable: false),
                    IdTecnico = table.Column<int>(type: "int", nullable: true),
                    FechaProgramada = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaRealizada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mantenimiento", x => x.IdMantenimiento);
                    table.ForeignKey(
                        name: "FK_Mantenimiento_PlanMantenimiento_IdPlan",
                        column: x => x.IdPlan,
                        principalTable: "PlanMantenimiento",
                        principalColumn: "IdPlan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mantenimiento_Tecnico_IdTecnico",
                        column: x => x.IdTecnico,
                        principalTable: "Tecnico",
                        principalColumn: "IdTecnico");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_IdTipoEquipo",
                table: "Equipo",
                column: "IdTipoEquipo");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_IdPlan",
                table: "Mantenimiento",
                column: "IdPlan");

            migrationBuilder.CreateIndex(
                name: "IX_Mantenimiento_IdTecnico",
                table: "Mantenimiento",
                column: "IdTecnico");

            migrationBuilder.CreateIndex(
                name: "IX_PlanMantenimiento_IdEquipo",
                table: "PlanMantenimiento",
                column: "IdEquipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mantenimiento");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "PlanMantenimiento");

            migrationBuilder.DropTable(
                name: "Tecnico");

            migrationBuilder.DropTable(
                name: "Equipo");

            migrationBuilder.DropTable(
                name: "Tipo_Equipo");
        }
    }
}
