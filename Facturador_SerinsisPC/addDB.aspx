<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="addDB.aspx.cs" Inherits="Facturador_SerinsisPC.addDB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .db-modal-page {
            min-height: calc(100vh - 150px);
            display: flex;
            align-items: center;
            justify-content: center;
            padding: .4rem 0 1rem;
        }

        .db-modal-shell {
            width: min(100%, 1080px);
            background: rgba(16, 27, 55, 0.18);
            border-radius: 24px;
            padding: 1rem;
            backdrop-filter: blur(4px);
        }

        .db-modal-card {
            background: linear-gradient(135deg, #fbfdff 0%, #f3f9ff 100%);
            border: 1px solid #cfe2f3;
            border-radius: 22px;
            overflow: hidden;
            box-shadow: 0 24px 48px rgba(42, 34, 166, 0.12);
        }

        .db-modal-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 1rem;
            padding: 1rem 1.1rem;
            background: linear-gradient(135deg, #2a22a6 0%, #62afe2 100%);
            color: #ffffff;
        }

        .db-modal-title {
            display: flex;
            align-items: center;
            gap: .7rem;
            font-size: 1.5rem;
            font-weight: 700;
            margin: 0;
        }

        .db-modal-close {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 42px;
            height: 42px;
            border-radius: 12px;
            color: #ffffff;
            background: rgba(255, 255, 255, 0.16);
            text-decoration: none;
            font-size: 1.15rem;
        }

        .db-modal-close:hover {
            color: #ffffff;
            text-decoration: none;
            background: rgba(255, 255, 255, 0.24);
        }

        .db-modal-body {
            padding: 1rem 1.1rem 1.1rem;
        }

        .db-modal-info {
            color: #61779c;
            font-size: .93rem;
            margin-bottom: .9rem;
        }

        .db-form-card {
            background: #ffffff;
            border: 1px solid #d7e5f3;
            border-radius: 16px;
            padding: .95rem 1rem;
            margin-bottom: .9rem;
        }

        .db-form-grid {
            display: grid;
            grid-template-columns: minmax(220px, .85fr) minmax(280px, 1.4fr) auto;
            gap: .8rem;
            align-items: end;
        }

        .db-field label {
            color: #2a22a6;
            font-weight: 700;
            font-size: .88rem;
            margin-bottom: .35rem;
            display: block;
        }

        .db-field .form-control {
            min-height: 42px;
            border: 1px solid #cfe2f3;
            border-radius: 12px;
            color: #243a60;
            box-shadow: none;
        }

        .db-field .form-control:focus {
            border-color: #62afe2;
            box-shadow: 0 0 0 .16rem rgba(98, 175, 226, 0.18);
        }

        .db-primary-btn {
            border-radius: 12px;
            padding: .6rem 1rem;
            font-weight: 700;
            background: linear-gradient(135deg, #3c35c6 0%, #2a22a6 100%);
            border: 0;
            box-shadow: 0 12px 22px rgba(42, 34, 166, 0.16);
        }

        .db-primary-btn:hover {
            transform: translateY(-1px);
            box-shadow: 0 14px 24px rgba(42, 34, 166, 0.18);
        }

        .db-table-wrap {
            background: #ffffff;
            border: 1px solid #d7e5f3;
            border-radius: 16px;
            padding: .3rem;
        }

        .db-table {
            width: 100%;
        }

        .db-state {
            display: inline-flex;
            align-items: center;
            gap: .45rem;
            font-weight: 700;
            padding: .35rem .6rem;
            border-radius: 999px;
            font-size: .84rem;
        }

        .db-state--activo {
            background: #ecfbf2;
            color: #1f9b53;
        }

        .db-state--inactivo {
            background: #fff0f2;
            color: #d8344a;
        }

        .db-option {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 34px;
            height: 34px;
            border-radius: 10px;
            text-decoration: none;
            background: #fff0f2;
            color: #d8344a;
            transition: transform .15s ease, box-shadow .15s ease;
        }

        .db-option:hover {
            text-decoration: none;
            transform: translateY(-1px);
            box-shadow: 0 8px 16px rgba(42, 34, 166, 0.10);
        }

        @media (max-width: 991.98px) {
            .db-form-grid {
                grid-template-columns: 1fr;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="db-modal-page">
        <div class="db-modal-shell">
            <div class="db-modal-card">
                <div class="db-modal-header">
                    <h1 class="db-modal-title"><i class="fas fa-database"></i> Bases de Datos</h1>
                    <a href="clientes.aspx" class="db-modal-close" title="Cerrar"><i class="fas fa-times"></i></a>
                </div>

                <div class="db-modal-body">
                    <p class="db-modal-info">Relaciona una base de datos con el cliente actual y administra las bases ya asociadas desde este panel.</p>

                    <section class="db-form-card">
                        <div class="db-form-grid">
                            <div class="db-field">
                                <label>Filtrar base</label>
                                <input type="text" id="txtFiltroDB" class="form-control" placeholder="Escribe para buscar..." oninput="filtrarBases()" />
                            </div>
                            <div class="db-field">
                                <label>Lista DB</label>
                                <asp:DropDownList runat="server" ID="ddl_db" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <asp:Button Text="Agregar" runat="server" ID="btn_Agregar" OnClick="btn_Agregar_Click" CssClass="btn btn-primary db-primary-btn" />
                        </div>
                    </section>

                    <section class="db-table-wrap">
                        <table id="tablaDB_Clientes" class="table-cebra db-table">
                            <thead>
                                <tr class="row-100">
                                    <th class="col-1">#</th>
                                    <th class="col-auto">Nombre DB</th>
                                    <th class="col-auto">Estado</th>
                                    <th class="col-auto">Opcion</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater runat="server" ID="rpDB_Cliente">
                                    <ItemTemplate>
                                        <tr class="row-100">
                                            <td class="col-1"><%# Container.ItemIndex +1 %></td>
                                            <td class="col-auto"><%# Eval("nameDataBase") %></td>
                                            <td class="col-auto">
                                                <span class='db-state <%# Convert.ToInt32(Eval("estado")) == 1 ? "db-state--activo" : "db-state--inactivo" %>'>
                                                    <i class='fas <%# Convert.ToInt32(Eval("estado")) == 1 ? "fa-check-circle" : "fa-pause-circle" %>'></i>
                                                    <%# Convert.ToInt32(Eval("estado")) == 1 ? "Activa" : "Inactiva" %>
                                                </span>
                                            </td>
                                            <td class="col-1">
                                                <asp:LinkButton runat="server" ID="btnEliminar" CssClass="db-option" ToolTip="Eliminar relación" OnClick="btnEliminar_Click" CommandArgument='<%# Eval("id") %>'><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </tbody>
                        </table>
                    </section>
                </div>
            </div>
        </div>
    </div>

    <script src="js/myScripts.js"></script>
    <script>
        var dbOptionsOriginales = [];

        document.addEventListener('DOMContentLoaded', function () {
            var ddl = document.getElementById('<%= ddl_db.ClientID %>');
            if (!ddl) return;

            dbOptionsOriginales = Array.prototype.map.call(ddl.options, function (opt) {
                return { value: opt.value, text: opt.text };
            });
        });

        function filtrarBases() {
            var ddl = document.getElementById('<%= ddl_db.ClientID %>');
            var filtro = document.getElementById('txtFiltroDB');
            if (!ddl || !filtro || !dbOptionsOriginales.length) return;

            var texto = filtro.value.toLowerCase().trim();
            ddl.options.length = 0;

            var coincidencias = dbOptionsOriginales.filter(function (item) {
                return item.text.toLowerCase().indexOf(texto) >= 0;
            });

            coincidencias.forEach(function (item) {
                ddl.options.add(new Option(item.text, item.value));
            });
        }
    </script>
</asp:Content>
