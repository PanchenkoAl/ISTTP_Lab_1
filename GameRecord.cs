using System;
using System.Collections.Generic;

namespace LIBWebApplication1;

public partial class GameRecord
{
    public int GameRecordId { get; set; }

    public int Team1Id { get; set; }

    public int Team2Id { get; set; }

    public DateTime GameDate { get; set; }

    public bool? Winner { get; set; }

    public string? Info { get; set; }

    public int TournamentId { get; set; }

    public virtual Team Team1 { get; set; } = null!;

    public virtual Team Team2 { get; set; } = null!;

    public virtual Tournament Tournament { get; set; } = null!;
}
