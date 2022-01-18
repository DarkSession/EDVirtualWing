import { StarSystem } from "./star-system";

export interface Station {
    Name: string;
    NameAddon: string | null;
    StarSystem: StarSystem;
    DistanceFromStarLS: number;
    StationType: StationType;
}

export enum StationType {
    Coriolis = 1,
    Outpost,
    Orbis,
    FleetCarrier,
    CraterOutpost,
    Ocellus,
    CraterPort,
    AsteroidBase,
    Bernal,
    MegaShip,
    SurfaceStation,
    OnFootSettlement,
}