export class SearchResultDto {
    public count: string;
    public Result: OptionDto[];
}

export class OptionDto {
    public TimeTaken: number;
    public Options: TrainDto[];

}
export class TrainDto {
    public TrainId: number;
    public TrainName: string;
    public Direction: string;
    public StationId: number;
    public StartStationName: string;
    public StartStationDeparture: number;
    public EndStationId: number;
    public EndStationName: string;
    public EndStationArrival: number;
    public Duration: number;
}
