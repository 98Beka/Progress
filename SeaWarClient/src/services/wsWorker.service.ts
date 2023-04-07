import {
  HubConnectionBuilder,
  HttpTransportType,
  HubConnection,
} from "@microsoft/signalr";
import type {
  IMessageDto,
  IInitGameDto,
  ShootResultDto,
} from "@/models/dto.model";
import { v4 as uuidv4 } from "uuid";
import { BehaviorSubject, filter, map, Subject, take } from "rxjs";

class WsWorker {
  /** ответ от сервера */
  answer$ = new Subject<IMessageDto<any>>();

  /** старт игры */
  startGame$ = new Subject<IInitGameDto>();

  /** результат выстрела */
  resultShoot$ = new Subject<IMessageDto<ShootResultDto>>();

  connection: HubConnection;

  /** текущий ID соединения */
  connectionId$ = new BehaviorSubject<string>(null);
  constructor(url: string) {
    this.connection = new HubConnectionBuilder()
      .withUrl(url, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .build();

    this.connection.start().then(() => {
      console.log("соединение по WS установленно");
      this.subscribeCmd("Answer", this.answer$);
      this.subscribeCmd("StartGame", this.startGame$);
      this.subscribeCmd("ResultShoot", this.resultShoot$);
    });
    this.connection.on("InitConnection", (connectionId: string) => {
      this.connectionId$.next(connectionId);
    });
  }
  /** отправка сообщения серверу */
  sendMsg<T>(commandName: string, payload: T) {
    const msgUid = uuidv4();
    this.connection.invoke(commandName, <IMessageDto<T>>{
      uid: msgUid,
      payload,
    });
    return msgUid;
  }

  /** ждём ответа на sendMsg */
  getAnswer$<T>(msgUid: string) {
    return this.answer$.pipe(
      filter((message) => message.uid === msgUid),
      take(1),
      map((message) => message.payload as T)
    );
  }

  private subscribeCmd<T>(cmdName: string, subj: Subject<T>) {
    this.connection.on(cmdName, (data: any) => {
      subj.next(data);
    });
  }
}

export const wsWorker = new WsWorker("http://localhost:5127/gameHub");