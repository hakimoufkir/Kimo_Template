export class Result<T = void> {
  readonly success: boolean;
  readonly data?: T;
  readonly error?: Error;

  private constructor(success: boolean, data?: T, error?: Error) {
    this.success = success;
    this.data = data;
    this.error = error;
  }

  static ok(): Result;
  static ok<T>(data: T): Result<T>;
  static ok<T>(data?: T): Result<T | void> {
    return new Result<T | void>(true, data);
  }

  static error<T = never>(error: Error): Result<T> {
    return new Result<T>(false, undefined, error);
  }
}
