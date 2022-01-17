import { StarSystem } from "./star-system";

export interface Commander {
    Name: string;
    GameActivity: GameActivity;
    CurrentStarSystem: StarSystem;
}

export enum GameActivity {
    None = 0,
    Supercruise,
    Hyperspace,
    Combat,
}