import { call, put, select, takeLatest } from 'redux-saga/effects';
import { makeSelectOidc } from 'containers/App/selectors';

import {
  post,
  get,
  postWithAccessToken,
  getWithAccessToken,
} from '../../api/axiosApi';
import {
  SAVE_POST_REQUEST,
  GET_MANAGE_POST_VIEW_REQUEST,
  UPLOAD_POST_COVER_IMAGE_REQUEST,
  GET_EDIT_POST_REQUEST,
} from './constants';
import {
  getManagePostViewSuccess,
  getManagePostViewError,
  uploadPostCoverImageSuccess,
  uploadPostCoverImageError,
  getEditPostSuccess,
  getEditPostError,
  savePostSuccess,
  savePostError,
} from './actions';

export default function* dashboardSaga() {
  yield takeLatest(SAVE_POST_REQUEST, savePostSaga);
  yield takeLatest(GET_MANAGE_POST_VIEW_REQUEST, getManagePostViewSaga);
  yield takeLatest(UPLOAD_POST_COVER_IMAGE_REQUEST, uploadPostCoverImageSaga);
  yield takeLatest(GET_EDIT_POST_REQUEST, getEditPostSaga);
}

export function* savePostSaga({ data }) {
  try {
    const response = yield call(postWithAccessToken, '/post', data);
    yield put(savePostSuccess(response));
  } catch (error) {
    yield put(savePostError(error));
  }
}

export function* getManagePostViewSaga({ page }) {
  try {
    const response = yield call(getWithAccessToken, `/post/managepost/${page}`);
    yield put(getManagePostViewSuccess(response));
  } catch (error) {
    yield put(getManagePostViewError(error));
  }
}

export function* uploadPostCoverImageSaga({ file }) {
  try {
    const formData = new FormData();
    formData.append('file', file);
    const config = { 'Content-Type': 'multipart/form-data' };
    const response = yield call(postWithAccessToken, `/file`, formData, config);
    yield put(uploadPostCoverImageSuccess(response));
  } catch (error) {
    yield put(uploadPostCoverImageError(error));
  }
}

export function* getEditPostSaga({ id }) {
  try {
    const response = yield call(getWithAccessToken, `/post/${id}`);
    yield put(getEditPostSuccess(response));
  } catch (error) {
    yield put(getEditPostError(error));
  }
}
