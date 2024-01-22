﻿using FFXIVClientStructs.FFXIV.Client.Game.Housing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBridge.Gui
{
    public unsafe static class HouseReg
    {
        public static void Draw()
        {
            var CurrentHouse = HousingManager.Instance()->GetCurrentHouseId();
            if (CurrentHouse > 0)
            {
                ImGuiEx.Text($"Current house: {CurrentHouse:X16}");
                if (!C.Houses.TryGetFirst(x => x.ID == CurrentHouse, out var record))
                {
                    if (ImGui.Button("Register this house"))
                    {
                        C.Houses.Add(new() { ID = CurrentHouse, Name = Utils.GetHouseDefaultName() });
                    }
                }
            }
            else
            {
                ImGuiEx.Text($"You are not in house");
            }
            if (ImGui.BeginTable("##houses", 3, ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Borders))
            {
                ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("ID");
                ImGui.TableSetupColumn(" ");
                ImGui.TableHeadersRow();
                foreach (var x in C.Houses)
                {
                    ImGui.PushID(x.GUID);
                    var col = x.ID == CurrentHouse;
                    if (col) ImGui.PushStyleColor(ImGuiCol.Text, EColor.GreenBright);

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();

                    ImGuiEx.SetNextItemFullWidth();
                    ImGui.InputText("##name", ref x.Name, 100);

                    ImGui.TableNextColumn();
                    ImGuiEx.Text($"{x.ID:X16}");

                    ImGui.TableNextColumn();
                    if (ImGuiEx.IconButton(FontAwesomeIcon.Trash))
                    {
                        new TickScheduler(() => C.Houses.RemoveAll(z => z.GUID == x.GUID));
                    }

                    if (col) ImGui.PopStyleColor();
                    ImGui.PopID();
                }
                ImGui.EndTable();
            }
        }
    }
}
