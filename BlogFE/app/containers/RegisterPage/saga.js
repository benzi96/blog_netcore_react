import { call, put, select, takeLatest } from 'redux-saga/effects';
import { post } from '../../api/axiosApi';
import { REGISTER_REQUEST } from './constants';
import { registerSuccess, registerError } from './actions';
import makeSelectRegisterPage from './selectors';

export default function* registerPageSaga() {
  yield takeLatest(REGISTER_REQUEST, register);
}

export function* register() {
  try {
    const registerData = yield select(makeSelectRegisterPage());
    const data = {
      userName: registerData.userName,
      password: registerData.password,
    };

    const response = yield call(post, '/identity', data);
    yield put(registerSuccess(response));
  } catch (error) {
    yield put(registerError(error));
  }
}
