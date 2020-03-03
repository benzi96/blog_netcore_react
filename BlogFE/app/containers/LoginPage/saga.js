import { call, put, select, takeLatest } from 'redux-saga/effects';
import { post } from '../../api/axiosApi';
import { LOGIN_REQUEST } from './constants';
import { loginSuccess, loginError } from './actions';
import makeSelectLoginPage from './selectors';

export default function* loginPageSaga() {
  yield takeLatest(LOGIN_REQUEST, login);
}

export function* login() {
  // try {
  //   const loginData = yield select(makeSelectLoginPage());
  //   const response = yield call(
  //     [axiosApi, axiosApi.getToken],
  //     loginData.userName,
  //     loginData.password,
  //   );
  //   const { tokenInfo, userInfo } = axiosApi.parseAccessToken(response);
  //   yield put(loginSuccess(response));
  // } catch (error) {
  //   yield put(loginError(error));
  // }
}
